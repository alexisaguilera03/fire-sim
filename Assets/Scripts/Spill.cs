using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{

   private ParticleSystem myParticleSystem;
    private Quaternion originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        originalPosition = gameObject.transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        int difference = (int)Mathf.Round(originalPosition.x - gameObject.transform.position.x);
        if(difference > 90f)
        {
            myParticleSystem.Play();
        }
        else
        {
            myParticleSystem.Stop();
        }
    }
}
