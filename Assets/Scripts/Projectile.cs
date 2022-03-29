using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{
    public Extinguisher extinguisher;

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == extinguisher.gameObject || collision.gameObject.GetComponent<Hand>()!=null)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            //todo: put in code for hitting fire
            Destroy(collision.gameObject);
        }
        extinguisher.StopAllCoroutines();
        Destroy(gameObject);
    }

}
