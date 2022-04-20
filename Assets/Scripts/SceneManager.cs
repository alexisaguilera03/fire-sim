using System;
using System.Collections;
using System.Collections.Generic;

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

    private Vector3? playerSpawnPosition;

    private Quaternion? playerSpawnRotation;



    // Start is called before the first frame update
    void Start()
    {
        getObjects();
        switch (currentScene.name)
        {
            case "Menu":
                updateWinCondition = null;
                winFunction = null;
                loseCondition = null;
                menu = true;
                nextScene = scene1;
                if (WristTextManager.Instance != null) WristTextManager.Instance.SetObjectiveText("Start your game or select which level you want to play!");
                break;
            case "Kitchen":
                playerSpawnPosition = new Vector3(-2.7f, 0.5f, -5.2f);
                playerSpawnRotation = Quaternion.Euler(0,180,0);
                menu = false;
                nextScene = scene2;
                updateWinCondition = () => winCondition.Fires = fireManager.fireCount;
                winFunction = () => winCondition.checkWinCondition(0);
                if (WristTextManager.Instance != null) WristTextManager.Instance.SetObjectiveText("Put out the grease fire on the stove with one of the highighted objects!");
                loseCondition.enforceMaxFires = true;
                loseCondition.maxFires = 10;
                loseCondition.maxTime = 120f;
                loseCondition.enforceMaxTime = true;
                break;
            case "Escape":
                //todo: update win condition for new scene
                winFunction = () => winCondition.checkWinCondition();
                updateWinCondition = () => winCondition.Win = winCondition.Win;  //effectively do nothing
                if(WristTextManager.Instance != null)WristTextManager.Instance.SetObjectiveText("Find your way out of the house, make sure to avoid fire and smoke!");
                loseCondition.enforceMaxTime = true;
                loseCondition.maxTime = 60 * 5;
                loseCondition.enforceMaxFires = false;
                
                nextScene = "";
                break;
        }
        SetSpawnPoint();
        fader.FadeOut(0.5f);
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

    void SetSpawnPoint()
    {
        if (playerSpawnPosition is null || playerSpawnRotation is null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = (Vector3)playerSpawnPosition;
        player.transform.rotation = (Quaternion)playerSpawnRotation;
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

    public void LoadFirstLevel()
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), currentScene);
        Invoke("Load", 1);
        
    }

    void Load()
    {
        levelLoader.Trigger();
    }

     public void reset()
    {
        nextScene = currentScene.name;
        StartLoad();

    }

     void setPlayerPosition()
     {

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
