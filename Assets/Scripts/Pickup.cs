using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class Pickup : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction = null;
    public SteamVR_Action_Boolean releaseAction = null;

    private bool isAttached;
    private bool softAttach;
    private Hand attachedHand;


    private Interaction interactionSystem;
    // Start is called before the first frame update
    void Start()
    {
        interactionSystem = interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (releaseAction.GetStateDown(attachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(attachedHand, gameObject, ref isAttached);
            attachedHand = null;
            isAttached = false;
        }
    }

    void OnHandHoverUpdate(Hand hand)
    {
        interactionSystem.OnHandHover(hand, gameObject, ref isAttached);
        if (!isAttached) return;
        attachedHand = hand;
        softAttach = true;
    }
}
