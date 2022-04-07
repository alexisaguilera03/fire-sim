using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject SmokeGameObject;
    public AudioSource FireAudioSource;
    private GameObject smoke;
    private SoundEngine soundEngine = null;
    private FireManager fireManager;
    private bool destroyFire = false;

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
    }

    public void stopFire()
    {
        if (destroyFire) return;
        destroyFire = true;
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
}