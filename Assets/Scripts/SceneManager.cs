using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class SceneManager : MonoBehaviour
{
    public string scene1 = "Kitchen";

    public string scene2 = "Escape";
    public Scene currentScene;
    public WinCondition winCondition;
    public LoseCondition loseCondition;
    public bool test = false;
    public bool load = false;
    public float maxTime = -1f;

    private string nextScene;

    private SteamVR_LoadLevel levelLoader;
    
    private Action winFunction;
    private Action updateWinCondition;

    private FireManager fireManager;

    private SoundEngine soundEngine;

    private Fade fader;

    private GameObject player;

    private bool loading = false;
    private bool menu = false;


    
    // Start is called before the first frame update
    void Start()
    {
        getObjects();
        fader.FadeOut(0.5f);
        switch (currentScene.name)
        {
            case "Menu":
                updateWinCondition = null;
                winFunction = null;
                loseCondition = null;
                menu = true;
                nextScene = scene1;
                break;
            case "Kitchen":
                menu = false;
                nextScene = scene2;
                updateWinCondition = () => winCondition.Fires = fireManager.fireCount;
                winFunction = () => winCondition.checkWinCondition(0);
                loseCondition.enforceMaxFires = true;
                loseCondition.maxFires = 10;
                loseCondition.maxTime = 120f;
                loseCondition.enforceMaxTime = true;
                break;
            case "Escape":
                //todo: update win condition for new scene
                //todo: update win condition updater for new scene
                //todo: update lose condition for new scene
                nextScene = "";
                break;
        }
        levelLoader.levelName = nextScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (test) StartLoad(); //remove when done
        //todo: fix loading not waiting for sound to finish


        if (menu) return;
        if (loseCondition.lost)
        {
            reset();
        }
        updateWinCondition();
        winFunction();
        if (load)
        {
            StartLoad();
        }
    }


    void StartLoad()
    {
        if (loading) return; //prevent duplicate call
        loading = true;
        Destroy(GameObject.FindGameObjectWithTag("InteractionSystem"));
        loseCondition.StopAllCoroutines();
        fader.FadeIn(Color.black, 1);
        Invoke("Load", 2);
    }

    void Load()
    {
        levelLoader.Trigger();
    }

     void reset()
    {
        nextScene = currentScene.name;
        StartLoad();

    }


     void getObjects()
     {
         soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
         fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
         levelLoader = gameObject.GetComponent<SteamVR_LoadLevel>();
         currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
         winCondition = gameObject.GetComponent<WinCondition>();
         loseCondition = gameObject.GetComponent<LoseCondition>();
         player = GameObject.FindGameObjectWithTag("MainCamera");
         fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
    }
}
