using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pan : MonoBehaviour
{
    public AudioSource success;
    public GameObject lid;
    public GameObject smoke;

    private bool isAttached;

    private Interaction interactionSystem = null;

    private SoundEngine soundEngine = null;
    private ParticleSystem smokeParticleSystem = null;


    // Start is called before the first frame update
    void Start()
    {
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        success = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
        smokeParticleSystem = smoke.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Hand hand = collider.transform.parent.gameObject.GetComponent<Hand>();
        if (collider.gameObject == lid)
        {
            interactionSystem.Release(hand, lid, ref isAttached);
            interactionSystem.AttachToObject(lid, gameObject, Vector3.up);
            soundEngine.PlaySoundEffect(success);
            smokeParticleSystem.Stop();
        }
    }
}
