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
    private FireManager fireManager;
    private SoundEngine soundEngine = null;
    private GameObject fire;
    private SceneManager sceneManager;



    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        fire = fireManager.createFireGameObject(transform.position, fireGameobject.transform.rotation);
        //todo: adjust positioning
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        
        
        if (collider.gameObject == lid)
        {
            
            if (fireManager.fireCount == 1)
            {
                sceneManager.winCondition.WinAudioSource = success;
                //sceneManager.winCondition.Win = true;
            }
            fire.GetComponent<Fire>().stopFire();
            Hand hand = collider.transform.parent.gameObject.GetComponent<Hand>();
            firePutOut = true;
            Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.001f, gameObject.transform.position.z);
            interactionSystem.Release(hand, lid, ref isAttached);
            interactionSystem.AttachToObject(lid, gameObject, position);

            //soundEngine.PlaySoundEffect(success, false, false);
            
            
        }
    }
}
