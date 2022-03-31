using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{
    public Extinguisher extinguisher;
    public AudioSource SuccessExtinguishAudioSource;
    private SoundEngine soundEngine;

    private void Start()
    {
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
    }
    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == extinguisher.gameObject || collision.gameObject.GetComponent<Hand>()!=null)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (SuccessExtinguishAudioSource != null)
            {
                soundEngine.PlaySoundEffectPriority(SuccessExtinguishAudioSource, false);
            }
            collision.gameObject.GetComponent<Fire>().stopFire();
        }
        extinguisher.StopAllCoroutines();
        Destroy(gameObject);
    }

}
