using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Door : MonoBehaviour
{
    public GameObject door;
    public GameObject[] doorKnobs;
    public GameObject deadBolt;
    private Interaction interactionSystem = null;

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
        doorOriginalRotation = door.transform.rotation;
        getInteractionSystem();
        setParents();
        setLimits();
    }

    // Update is called once per frame
    void Update()
    {
      
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


}
