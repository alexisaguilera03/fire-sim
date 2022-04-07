using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class SceneManager : MonoBehaviour
{
    private struct WinCondition
    {
        public int Fires { get; set; }
        public bool Win { get; set; }

        public WinCondition(int fire = -1, bool won = false)
        {
            Fires = fire;
            Win = won;
        }

        public bool checkWinCondition(int target = -1)
        {
            if (this.Fires <= target && target != -1)
            {
                this.Win = true;
            }

            return this.Win;
        }


    }
    public string scene1 = "Kitchen";

    public string scene2 = "Escape";
    public Scene currentScene;

    public bool test = false;
    private string nextScene;
    private SteamVR_LoadLevel levelLoader;
    private  WinCondition winCondition;
    private Action winFunction;
    private FireManager fireManager;

    
    // Start is called before the first frame update
    //todo: assign objects to use scene manager
    void Start()
    {
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
                winCondition = new WinCondition(winCondition.Fires, default);
                nextScene = "";
                break;
        }
        levelLoader.levelName = nextScene;
    }

    // Update is called once per frame
    void Update()
    {
        winCondition.Fires = fireManager.fireCount;
        winFunction();
    }
}
