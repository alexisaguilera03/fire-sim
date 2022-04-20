using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Door : MonoBehaviour
{
    public bool deathDoor = false;
    public GameObject door;
    public GameObject[] doorKnobs;
    public GameObject deadBolt;
    private Interaction interactionSystem = null;
    private Fade fader;
    private bool playerInteracted = false;
    private bool dead = false;

    private Quaternion doorOriginalRotation;

    // Start is called before the first frame update
    void Awake()
    {

        if (doorKnobs.Length < 1)
        {
            Debug.LogError("Doorknobs must be assigned for door functionality");
        }
    }
    void Start()
    {
        //playerInteracted = true; //debug remove when done
        gameObject.GetComponent<Collider>().isTrigger = true;
        fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
        doorOriginalRotation = door.transform.rotation;
        getInteractionSystem();
        setParents();
        setLimits();
        if (deathDoor)
        {
            setDoorknobColor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deathDoor)
        {
            checkDeathDoor();
            //todo: create function to give haptic feedback to the player if they get close to the knob
        }
        
    }
    void getInteractionSystem()
    {
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }
    }

    void setParents()
    {
        if (deadBolt != null)
        {
            deadBolt.transform.parent = door.transform;
        }
        foreach (GameObject knob in doorKnobs)
        {
            knob.transform.parent = door.transform;
        }
    }
    void setLimits()
    {
        HingeJoint joint = door.GetComponent<HingeJoint>();
        JointLimits limits = joint.limits;
        limits.min = -90;
        limits.max = 90;
        joint.limits = limits;
        joint.useLimits = true;

    }

    void setDoorknobColor()
    {
        foreach (GameObject knob in doorKnobs)
        {
            foreach (MeshRenderer rend in knob.GetComponentsInChildren<MeshRenderer>())
            {
                rend.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, rend.GetComponent<MeshRenderer>().material.color, Mathf.PingPong(Time.time, 1) / 1);
            }
            
            
        }
    }

    void checkDeathDoor()
    {
        if (!playerInteracted) return;
        Quaternion currentRotation = gameObject.transform.rotation;
        if (currentRotation != doorOriginalRotation)
        {
            killPlayer();
        }
    }

    void killPlayer()
    {
        if (dead) return;

        dead = true;
        var explostion = Instantiate(GameObject.FindGameObjectWithTag("Explosion"),GameObject.FindGameObjectWithTag("MainCamera").transform.position + Vector3.forward/5, GameObject.FindGameObjectWithTag("MainCamera").transform.rotation);
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().loseCondition.lose = true;
        GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>().FadeIn(Color.white, 1);
        Destroy(explostion, 1f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.root.gameObject.CompareTag("Player"))
        {
            playerInteracted = true;
            gameObject.GetComponent<Collider>().isTrigger = false; //todo: make less janky
        }
    }


}
