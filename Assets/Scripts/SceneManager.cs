using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class SceneManager : MonoBehaviour
{
    public struct WinCondition
    {
        public int Fires { get; set; }
        public bool Win { get; set; }
        public AudioSource WinAudioSource { get; set; }

        public WinCondition(int fire = -1, bool won = false, AudioSource winAudioSource = null)
        {
            Fires = fire;
            Win = won;
            WinAudioSource = winAudioSource;
        }

        public bool checkWinCondition(int target = -1)
        {
            if (this.Fires <= target && target != -1)
            {
                this.Win = true;
            }

            return this.Win;
        }

        public bool checkWinCondition()
        {
            return this.Win;
        }

        public bool win(SoundEngine soundEngine)
        {
            if (WinAudioSource != null)
            {
                if (soundEngine.checkPlaying(WinAudioSource))
                {
                    return true;
                }
                soundEngine.PlaySoundEffectPriority(WinAudioSource, false);
                return soundEngine.checkPlaying(WinAudioSource);

            }

            return false;
        }


    }
    public string scene1 = "Kitchen";

    public string scene2 = "Escape";
    public Scene currentScene;

    public bool test = false;
    private string nextScene;
    private SteamVR_LoadLevel levelLoader;
    public WinCondition winCondition;
    private Action winFunction;
    private FireManager fireManager;
    private SoundEngine soundEngine;

    
    // Start is called before the first frame update
    //todo: assign objects to use scene manager
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        levelLoader = gameObject.GetComponent<SteamVR_LoadLevel>();
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "Kitchen":
                nextScene = scene2;
                winCondition = new WinCondition(0, default);
                winFunction = () => winCondition.checkWinCondition(0);
                break;
            case "Escape":
                //todo: update win condition for new scene
                winCondition = new WinCondition(winCondition.Fires, default);
                nextScene = "";
                break;
        }
        levelLoader.levelName = nextScene;
    }

    // Update is called once per frame
    void Update()
    {
        //todo: fix loading not waiting for sound to finish
        //todo: implement steamvr fade
        winCondition.Fires = fireManager.fireCount;
        winFunction();
        if (!winCondition.Win) return;

        if (!winCondition.win(soundEngine))
        {
            levelLoader.Trigger();
        }
    }
}
