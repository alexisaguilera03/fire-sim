using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(collision.gameObject == extinguisher.gameObject)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            //todo: put in code for hitting fire
        }
        Destroy(gameObject);
    }

}
