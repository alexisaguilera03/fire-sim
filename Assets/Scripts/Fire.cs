using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject SmokeGameObject;
    public AudioSource FireAudioSource;
    private GameObject smoke;
    private SoundEngine soundEngine = null;
    private bool destroyFire = false;

    // Start is called before the first frame update
    void Start()
    {
        smoke = Instantiate(SmokeGameObject, transform.position, Quaternion.Euler(0,0,0));
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
        
    }

    public void stopFire()
    {
        if (destroyFire) return;
        destroyFire = true;
        if (soundEngine.fireActive)
        {
            soundEngine.StopSound(FireAudioSource);
            soundEngine.fireActive = false;
        }
        smoke.GetComponentInChildren<ParticleSystem>().Stop();
        gameObject.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(waitToDestroy());


    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
