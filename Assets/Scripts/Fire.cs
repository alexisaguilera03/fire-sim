using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Fire : MonoBehaviour
{
    public GameObject SmokeGameObject;
    public AudioSource FireAudioSource;
    public bool preventHandsFire;
    private GameObject smoke;
    private SoundEngine soundEngine = null;
    private FireManager fireManager;
    private bool destroyFire = false;
    private bool handsOnFire = false;
    private bool delay;

    // Start is called before the first frame update
    void Start()
    {
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        smoke = Instantiate(SmokeGameObject, transform.position, Quaternion.Euler(0,0,0), transform);
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        if (!soundEngine.fireActive)
        {
            soundEngine.PlaySoundEffect(FireAudioSource, true, false);
            soundEngine.fireActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireManager.spread(gameObject.transform.position);
        soundEngine.fireActive = true;
        if (preventHandsFire && !delay)
        {
            StartCoroutine(wait());
        }
    }

    public void stopFire()
    {
        if (destroyFire) return;
        destroyFire = true;
        if (fireManager.fireCount - 1 <= 0 && soundEngine.fireActive)
        {
            soundEngine.StopSound(FireAudioSource);
            soundEngine.fireActive = false;
        }

        gameObject.GetComponent<Collider>().enabled = false;
        smoke.GetComponentInChildren<ParticleSystem>().Stop();
        gameObject.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(waitToDestroy());


    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (fireManager.fireCount > 0)
        {
            fireManager.fireCount--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Hand")) return;
        if (GameManager.Instance.currentLevel == "FireFighter") return;
        if (collision.gameObject.name.ToLower() == "pan")
        {
            preventHandsFire = true;
            return;
        }
        if (collision.gameObject.transform.Find("FryingPanFBX") != null) return;
        if (collision.gameObject.GetComponentInChildren<Spill>() != null) return;
        if (handsOnFire) return;
        if (preventHandsFire) return;
        handsOnFire = true;
        var tmp = fireManager.createFireGameObject(collision.gameObject.transform.position, Quaternion.Euler(0,0,0));
        tmp.GetComponent<Collider>().enabled = false;
        tmp.transform.SetParent(collision.gameObject.transform);
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().loseCondition.handsOnFire = true;
        print("lose");
    }

    IEnumerator wait()
    {
        delay = true;
        yield return new WaitForSeconds(1);
        preventHandsFire = false;
        delay = false;
    }
}
