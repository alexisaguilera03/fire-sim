using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Lifetime = 5.0f;

    private Rigidbody m_Ridigbody = null;
    
    private void Awake()
    {
        m_Ridigbody = GetComponent<Rigidbody>();
        SetInnactive();
    }

    public void Launch(Extinguisher extinguisher)
    {
        Debug.Log("Launch method ran");
        //transform.position = extinguisher.m_Barrel.position;
        //transform.rotation = extinguisher.m_Barrel.rotation;

        this.gameObject.SetActive(true);

        //m_Ridigbody.AddRelativeForce(Vector3.forward * extinguisher.m_Force, ForceMode.Impulse);
        StartCoroutine(TrackLifetime());
    }

    private IEnumerator TrackLifetime()
    {
        yield return new WaitForSeconds(m_Lifetime);
        SetInnactive();
    }

    public void SetInnactive()
    {
        m_Ridigbody.velocity = Vector3.zero;
        m_Ridigbody.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }
}
