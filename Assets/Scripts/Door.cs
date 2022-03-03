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
    private bool rotate = false;
    private bool open = false;
    private Quaternion doorOriginalRotation;

    // Start is called before the first frame update
    void Start()
    {
        doorOriginalRotation = door.transform.rotation;
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        deadBolt.transform.parent = door.transform;
        foreach (GameObject knob in doorKnobs)
        {
            knob.transform.parent = door.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate && !open)
        {
            openDoor();
        }

        else if (rotate && open)
        {
            closeDoor();
        }
    }

    void HandHoverUpdate(Hand hand)
    {

        interactionSystem.DoorHandHover(hand, ref rotate);
    }

    void openDoor()
    {
        StartCoroutine(Rotate());
        door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(door.transform.rotation.x, door.transform.rotation.y + 90, door.transform.rotation.z), 250 * Time.deltaTime);
    }

    void closeDoor()
    {
        StartCoroutine(Rotate());
        door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, doorOriginalRotation, 250 * Time.deltaTime);
    }
    void updateDoorStatus()
    {
        foreach(GameObject doorknob in doorKnobs)
        {
            Door[] components = doorknob.GetComponentsInChildren<Door>();
            foreach (Door door in components)
            {
                if(door.gameObject == gameObject)
                {
                    continue;
                }
                door.open = open;
            }
        }
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(2);
        rotate = false;
        open = !open;
        updateDoorStatus();
    }

}
