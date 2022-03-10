using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Interaction : MonoBehaviour
{

    private Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;
    public SteamVR_Action_Boolean grabAction;

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
        GrabTypes bestGrabType = hand.GetBestGrabbingType(GrabTypes.Pinch); //this line is used instead to of a SteamVR_Action_Boolean to allow 2d testing as well
        
        if ( bestGrabType != GrabTypes.None && !attached)
        {
            
            hand.AttachObject(objectToAttach, bestGrabType, attachmentFlags);
            attached = true;

        }
        else
        {
            attached = false;
            return;
        }

    }

    public void Release(Hand hand, GameObject objectToDetach, ref bool attached)
    {
        hand.DetachObject(objectToDetach, true);
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

    public void AttachToObject(GameObject heldObject, GameObject newObject, Vector3 offset)
    {
        heldObject.transform.parent = newObject.transform;
        heldObject.transform.rotation = newObject.transform.rotation;
        heldObject.transform.position = newObject.transform.position;
        heldObject.transform.position += offset;
    }

}


