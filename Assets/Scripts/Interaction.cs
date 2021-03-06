using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Interaction : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;
    private Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

    private HintSystem hintSystem;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHandHover( Hand hand, GameObject objectToAttach, ref bool attached)
    {
        if (attached) return;
        GrabTypes bestGrabType = hand.GetBestGrabbingType(GrabTypes.Pinch); //this line is used instead to of a SteamVR_Action_Boolean to allow 2d testing as well
        
        if ( bestGrabType != GrabTypes.None && !attached || grabAction.GetState(hand.handType) )
        {
            hintSystem = GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>();
            if (hintSystem.activeHint == HintSystem.Hint.GrabObjects)
            {
                hintSystem.hintTaken = true;
            }
            hand.AttachObject(objectToAttach, bestGrabType, attachmentFlags);
            attached = true;

        }
        else
        {
            attached = false;
            return;
        }

    }

    void addRigidBody()
    {
        if (gameObject.GetComponent<Rigidbody>() != null) return;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    public void Release(Hand hand, GameObject objectToDetach, ref bool attached)
    {
        hand.DetachObject(objectToDetach, true);
        if (objectToDetach.GetComponent<Rigidbody>() is  null)
        {
            objectToDetach.gameObject.AddComponent<Rigidbody>();
        }

        Rigidbody objectRigidBody = objectToDetach.GetComponent<Rigidbody>();
        if (!objectRigidBody.useGravity)
        {
            objectRigidBody.useGravity = true;
        }

        if (objectRigidBody.isKinematic)
        {
            objectRigidBody.isKinematic = false;
        }

        attached = false;

    }

    public void DoorHandHover(Hand hand, ref bool rotate)
    {
        GrabTypes bestGrabType = hand.GetBestGrabbingType(GrabTypes.Pinch);
        if (bestGrabType != GrabTypes.None)
        {
            rotate = true;
            return;

        }
        
    }

    public void AttachToObject(GameObject heldObject, GameObject newObject, Vector3 position)
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        //heldObject.transform.parent = newObject.transform;
        heldObject.transform.rotation = newObject.transform.rotation;
        heldObject.transform.position = position;
    }

    public bool checkAttached(GameObject newObject)
    {
        Pickup pickup = newObject.GetComponent(typeof(Pickup)) as Pickup;
        return pickup.isAttached;
    }

    public bool checkIfHolding(Hand hand, ref bool isAttached)
    {
        GrabTypes bestGrabType = hand.GetBestGrabbingType(GrabTypes.Pinch);
        if (bestGrabType == GrabTypes.None)
        {
            return false;
        }

        return true;

    }

}


