using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public int maxFires;
    public float maxTime = -1f;

    public bool enforceMaxFires = false;
    public bool enforceMaxTime = false; 
    public bool lose = false;
    public bool lost = false;
    public bool handsOnFire = false;

    private SceneManager sceneManager;

    private FireManager fireManager;

    private SoundEngine soundEngine;

    private bool losing = false;

    private bool timing = false; 
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();


    }

    // Update is called once per frame
    void Update()
    {
        if(handsOnFire) startLose();
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
        if (explosion)
        {
            lost = true;
            //do stuff for death
        }
    }
    public void checkLoseCondition()
    {
        if (enforceMaxFires)
        {
            if (fireManager.fireCount >= maxFires)
            {
                lose = true;
            }
        }

    }

    void startLose() //todo: implement effects for explosion in kitchen and hands catching on fire
    {
        if (losing) return; 
        losing = true;

        //do stuff
        lost = true;
    }

    private IEnumerator startTimer(float seconds)
    {
        timing = true;
        yield return new WaitForSeconds(seconds);
        lose = true;
    }
}