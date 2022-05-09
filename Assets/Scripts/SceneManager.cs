using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public string scene1 = "Kitchen";

    public string scene2 = "Escape";
    public string currentScene;
    public WinCondition winCondition;
    public LoseCondition loseCondition;
    public bool test = false;
    public bool load = false;
    public float maxTime = -1f;
    public GameObject loadingScreen;

    private string nextScene;

    private GameManager gameManager;
    
    private Action winFunction;
    private Action updateWinCondition;

    private FireManager fireManager;

    private SoundEngine soundEngine;

    private Fade fader;

    private PlayerManager playerManager;

    private GameObject player;

    private bool loading = false;
    private bool menu = false;





    // Start is called before the first frame update
    void Start()
    {
        getObjects();
        switch (currentScene)
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
                menu = false;
                nextScene = scene2;
                updateWinCondition = () => winCondition.Fires = fireManager.fireCount;
                winFunction = () => winCondition.checkWinCondition(0);
                if (WristTextManager.Instance != null) WristTextManager.Instance.SetObjectiveText("Put out the grease fire on the stove with one of the highlighted objects!");
                loseCondition.enforceMaxFires = true;
                loseCondition.maxFires = 10;
                loseCondition.maxTime = 120f;
                loseCondition.enforceMaxTime = true;
                loseCondition.enforceHandsOnFire = true;
                break;
            case "Escape":
                winFunction = () => winCondition.checkWinCondition(default);
                updateWinCondition = () => winCondition.Fires = fireManager.fireCount; 
                if(WristTextManager.Instance != null)WristTextManager.Instance.SetObjectiveText("Find your way out of the house, make sure to avoid fire and smoke!");
                loseCondition.enforceMaxTime = true;
                loseCondition.maxTime = 60 * 5;
                loseCondition.enforceMaxFires = false;
                loseCondition.enforceHandsOnFire = true;
                playerManager.startPosition = new Vector3(21.792f, 2.55f, -28.82f);
                playerManager.startRotation = Quaternion.Euler(0,90,0);
                nextScene = "FireFighter";
                break;
            case "FireFighter":
                winFunction = () => winCondition.checkWinCondition(0);
                updateWinCondition = () => winCondition.Fires = fireManager.fireCount;
                if(WristTextManager.Instance != null)WristTextManager.Instance.SetObjectiveText("Put out the fire as quickly as possible!");
                loseCondition.enforceMaxTime = true;
                loseCondition.maxTime = 60 * 10;
                loseCondition.enforceMaxFires = false;
                loseCondition.enforceHandsOnFire = false;
                playerManager.startPosition = new Vector3(20.83f, 0.88f, -48.87f);
                playerManager.startRotation = Quaternion.Euler(0,0,0); 
                nextScene = "Credits";
                break;
            default:
                throw new UnityException("No options for scene " + currentScene);
        }

        gameManager.nextLevel = nextScene;
       // fader.FadeOut(0.5f);  //update this
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
            LevelSelect("Kitchen"); //remove when done
        }

        if (menu) return;
        if (loseCondition.lost)
        {
            reset();
        }
        updateWinCondition();
        winFunction();
        if (load)
        {
            Load();
        }
    }

    void StartLoad()
    {
        if (loading) return; //prevent duplicate call
        loading = true;
        Destroy(GameObject.FindGameObjectWithTag("InteractionSystem"));
        loseCondition.StopAllCoroutines();
        fader.FadeIn(Color.black, 1);
        Invoke("createLoadingScreen", 1);
        Invoke("Load", 2);
    }

    public void LoadFirstLevel()
    {
        test = false;
        gameManager.LoadFirstLevel();
    }

    public void LevelSelect(string levelName)
    {
        gameManager.LevelSelect(levelName);

    }

    void Load()
    {
        if (loading) return;
        loading = true;
        gameManager.Load();
    }

     public void reset()
    {
        if (loading) return;
        loading = true;
        nextScene = gameManager.currentLevel;
        gameManager.reset();

    }

     void createLoadingScreen()
     {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        GameObject sceneLoadingScreen = GameObject.Find("LoadingArea");
        if (sceneLoadingScreen is null)
        {
            sceneLoadingScreen = Instantiate(loadingScreen, new Vector3(300, 300), Quaternion.identity);
        }
        else
        {
            sceneLoadingScreen.GetComponentInChildren<Camera>().enabled = true;
        }

     }

     void getObjects()
     {
         soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
         fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
         gameManager = GameManager.Instance;
         currentScene = gameManager.currentLevel;
         winCondition = gameObject.GetComponent<WinCondition>();
         loseCondition = gameObject.GetComponent<LoseCondition>();
         fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
         playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
     }
}
