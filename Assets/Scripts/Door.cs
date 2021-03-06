using System.Collections;
using UnityEngine;

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
        if (GameManager.Instance.currentLevel == "FireFighter")
        {
            deathDoor = false;
        }
        //playerInteracted = true; //debug remove when done
        gameObject.GetComponentInChildren<Collider>().isTrigger = true;
        doorOriginalRotation = door.transform.rotation;
        setParents();
        setLimits();
        if (deathDoor)
        {
            setDoorknobColor();
        }

        if (!Load.ready) return;
        getInteractionSystem();
    }

    // Update is called once per frame
    void Update()
    {
        if (fader is null)
        {
            if (Load.ready)
            {
                if (GameObject.FindGameObjectWithTag("UI") == null) return;
                fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
                if(interactionSystem is null) getInteractionSystem();
            }
        }
        if (deathDoor)
        {
            checkDeathDoor();
            //todo: create function to give haptic feedback to the player if they get close to the knob
        }
        
    }
    void getInteractionSystem()
    {
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null && GameObject.FindGameObjectWithTag("InteractionSystem") != null)
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
        var explosion = Instantiate(GameObject.FindGameObjectWithTag("Explosion"),GameObject.FindGameObjectWithTag("MainCamera").transform.position + Vector3.forward/5, GameObject.FindGameObjectWithTag("MainCamera").transform.rotation);
        
        Destroy(explosion, 1f);
        StartCoroutine(showDeathHint());
    }

    IEnumerator showDeathHint()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>().FadeIn(Color.black, 1);
        GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>().displayHint(0,5);
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>().hint != HintSystem.Hint.None);
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>().hint == HintSystem.Hint.None);
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().loseCondition.lose = true;
        GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>().FadeIn(Color.white, 1);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.root.gameObject.CompareTag("Player"))
        {
            playerInteracted = true;
            gameObject.GetComponentInChildren<Collider>().isTrigger = false; //todo: make less janky
        }
    }


}
