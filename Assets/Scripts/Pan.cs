using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pan : MonoBehaviour
{
    public AudioSource success;
    public GameObject lid;
    public GameObject smokeGameObject;
    public GameObject fireGameobject;

    private bool isAttached;
    private bool firePutOut = false;

    private Interaction interactionSystem = null;

    private SoundEngine soundEngine = null;
    private GameObject fire;



    // Start is called before the first frame update
    void Start()
    {
        //todo: adjust positioning
        fire = Instantiate(fireGameobject, transform.position, fireGameobject.transform.rotation);
        //smoke = Instantiate(smokeGameObject, transform.position, transform.rotation);
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        //success = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
        


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
            firePutOut = true;
            Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.001f, gameObject.transform.position.z);
            interactionSystem.Release(hand, lid, ref isAttached);
            interactionSystem.AttachToObject(lid, gameObject, position);
            soundEngine.PlaySoundEffect(success, false, false);
            fire.GetComponent<Fire>().stopFire();
            //smoke.GetComponentInChildren<ParticleSystem>().Stop();
            //fire.GetComponent<ParticleSystem>().Stop();
            //smokeParticleSystem.Stop();
        }
    }
}
