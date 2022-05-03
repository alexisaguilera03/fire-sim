using System.Collections;
using UnityEngine;

public class CreditScroller : MonoBehaviour
{
    
    public GameObject logo;
    public GameObject text;
    
    private RectTransform rect;
    public float speed;

    void Start()
    {
        rect = text.GetComponent<RectTransform>();
    }
    void Update()
    {
        if(rect.anchoredPosition.y <= 1440)
        {
            MoveObjects();
        }
    }

    void MoveObjects()
    {
        logo.transform.Translate(Vector3.up * speed);
        text.transform.Translate(Vector3.up * speed);
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(.01f);
    }
}
