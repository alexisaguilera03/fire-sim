using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pan : MonoBehaviour
{
    public AudioSource success;
    public GameObject lid;
    private bool isAttached;
    private Interaction interactionSystem = null;
    // Start is called before the first frame update
    void Start()
    {
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }
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
        }
    }
}