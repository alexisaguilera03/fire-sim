using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Quaternion startRotation;
    public Quaternion endRotation;
    public bool rotateTowards;
    public bool moveTowards;
    public bool wakeUp;

    // Start is called before the first frame update
    void Start()
    {
        player = Resources.FindObjectsOfTypeAll<Player>()[0].gameObject;
        if(wakeUp) WakeUp();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WakeUp()
    {
        //todo: add fading
        gameObject.transform.position = new Vector3(22.2f,4.07f,-29.956f);
        gameObject.transform.rotation = Quaternion.Euler(0,90,0);
        Camera camera = gameObject.AddComponent<Camera>();
        StartCoroutine(getUp());
    }

    IEnumerator getUp()
    {
        float progress = 0f;
        yield return new WaitForSeconds(1f);
        while (gameObject.transform.rotation.eulerAngles != new Vector3(0, 0, 0))
        {
            progress += Time.deltaTime / 4;
            gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), progress);
            yield return null;
        }

        Vector3 pos = new Vector3(22.2f, 4.07f, -28.9f);
        progress = 0f;
        while (gameObject.transform.position != pos)
        {
            progress += Time.deltaTime / 50;
            gameObject.transform.position = Vector3.MoveTowards(transform.position, pos, progress);
            yield return null;
        }

        pos = new Vector3(22.2f, 4.5f, -28.9f);
        progress = 0f;
        while (gameObject.transform.position != pos)
        {
            progress += Time.deltaTime / 50;
            gameObject.transform.position = Vector3.MoveTowards(transform.position, pos, progress);
            yield return null;
        }
        Quaternion rotation = Quaternion.Euler(0,90,0);
        progress = 0f;
        while (gameObject.transform.rotation.eulerAngles != rotation.eulerAngles)
        {
            progress += Time.deltaTime;
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, progress);
            yield return null;
        }
        Destroy(gameObject.GetComponent<Camera>());
        player.transform.rotation = transform.rotation;
        player.SetActive(true);
        Load.ready = true;
        GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>().FadeOut(1f);
        

    }
}
