using System.Collections;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public int Fires = 0;
    public bool Win = false;
    private bool Won = false;
    private bool playing;
    public AudioSource WinAudioSource;

    private SoundEngine soundEngine;
    private SceneManager sceneManager;

// Start is called before the first frame update
    void Start()
    {
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        sceneManager.load = Won;
    }

    public bool checkWinCondition()
    {
        return Win;
    }
    

    public void checkWinCondition(int target = -1)
    {
        if (this.Fires <= target && target != -1)
        {
            Win = true;
        }

        if (Win)
        {
            startWin();

        }


    }
    private void startWin()
    {
        print("Won! moving to next scene");

        if (WinAudioSource != null && WinAudioSource.clip != null)
        {
            if (playing) return;
            playing = true;
            soundEngine.PlaySoundEffectPriority(WinAudioSource, false);
            StartCoroutine(waitUntilFinishedPlaying(WinAudioSource));

        }
        else
        {
            sceneManager.load = true;
        }
    }

    private IEnumerator waitUntilFinishedPlaying(AudioSource sound)
    {
        yield return new WaitWhile(() => sound.isPlaying);
        Won = true;
        sceneManager.load = true;
    }
}

