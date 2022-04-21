using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroller : MonoBehaviour
{
    
    public GameObject logo;
    public GameObject text;

    public float speed;

    void Update()
    {
        logo.transform.Translate(Vector3.up * speed);
        text.transform.Translate(Vector3.up * speed);
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(.01f);
    }
}
