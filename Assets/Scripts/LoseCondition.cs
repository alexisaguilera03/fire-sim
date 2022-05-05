using System.Collections;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public int maxFires;
    public float maxTime = -1f;

    public bool enforceMaxFires = false;
    public bool enforceMaxTime = false;
    public bool enforceHandsOnFire = true;
    public bool lose = false;
    public bool lost = false;
    public bool handsOnFire = false;
    public AudioSource LoseAudioSource;


    private SceneManager sceneManager;

    private FireManager fireManager;

    private SoundEngine soundEngine;

    private Fade fader;

    private HintSystem hintSystem;


    private bool losing = false;
    private bool timing = false; 
    // Start is called before the first frame update
    void Start()
    {
        getObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (handsOnFire && enforceHandsOnFire)
        {
            handsOnFire = false;
            GameManager.Instance.handsOnFire = true;
            fireManager.Invoke("ExtinguishAllFires", 10);
            fader.FadeIn(Color.white, 2, 10);
            Invoke("startLose", 11);
        }
        if(lose) startLose();
        if (!timing && enforceMaxTime && maxTime > 0)
        {
            StartCoroutine(startTimer(maxTime));
        }
    }

    void LateUpdate()
    {
        checkLoseCondition();

    }

    public void setLost(bool explosion)
    {
        if (explosion && LoseAudioSource != null)
        {
            StartCoroutine(playDeathSound());
        }
        else
        {
            lose = true;
        }
    }

    IEnumerator playDeathSound()
    {
        soundEngine.PlaySoundEffectPriority(LoseAudioSource, false);
        yield return new WaitWhile(() => LoseAudioSource.isPlaying);
        lose = true;
    }
    public void checkLoseCondition()
    {
        if (!enforceMaxFires) return;

        if (fireManager.fireCount < maxFires) return;
        GameManager.Instance.TimedOut = true;
        lose = true;

    }

    void startLose() //todo: implement effects for explosion in kitchen and hands catching on fire
    {
        if (losing) return;
        losing = true;
        hintSystem.removeHint();
        

        //do stuff
        lost = true;
        sceneManager.reset();
    }

    private IEnumerator startTimer(float seconds)
    {
        timing = true;
        yield return new WaitForSeconds(seconds);
        GameManager.Instance.TimedOut = true;
        lose = true;
    }

    void getObjects()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
        hintSystem = GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>();
    }
}
