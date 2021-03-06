using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Outline))]

public class Pickup : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean releaseAction;

    public bool isAttached = false;
    public bool softAttach;
    private Hand attachedHand;


    private Interaction interactionSystem;
    // Start is called before the first frame update
    void Start()
    {
        interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        gameObject.transform.SetParent(interactionSystem.transform, true);
        gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isAttached) return;
        if (softAttach)
        {
            if (!interactionSystem.checkIfHolding(attachedHand, ref isAttached))
            {
                interactionSystem.Release(attachedHand, gameObject, ref isAttached);
                attachedHand = null;
                isAttached = false;
                return;
            }
        }
        if (releaseAction.GetState(attachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(attachedHand, gameObject, ref isAttached);
            attachedHand = null;
            isAttached = false;
        }
    }

    void HandHoverUpdate(Hand hand)
    {
        interactionSystem.OnHandHover(hand, gameObject, ref isAttached);
        if (!isAttached) return;
        attachedHand = hand;
    }
}
