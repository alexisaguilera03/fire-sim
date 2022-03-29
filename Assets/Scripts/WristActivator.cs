using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristActivator : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject wristMenu;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        wristMenu.SetActive(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask));
    }
}
