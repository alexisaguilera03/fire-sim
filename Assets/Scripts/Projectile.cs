using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{

    public Extinguisher extinguisher;
    public GameObject shotFrom;
    public int index;
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
        //todo: remove win condition
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (SuccessExtinguishAudioSource != null)
            {
                soundEngine.PlaySoundEffectPriority(SuccessExtinguishAudioSource, false);
            }
            collision.gameObject.GetComponent<Fire>().stopFire();
        }

        if (extinguisher != null)
        {
            extinguisher.projectileActive = false;
            extinguisher.StopAllCoroutines();
        }

        if (shotFrom != null)
        {
            MonoBehaviour[] scripts = shotFrom.GetComponents<MonoBehaviour>();
            scripts[index].StopAllCoroutines();
        }
        Destroy(gameObject);
    }

}
