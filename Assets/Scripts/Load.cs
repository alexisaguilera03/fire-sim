using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{

    public GameObject MainGameObjects;

    public List<GameObject> EssentialGameObjects = new List<GameObject>();

    public static bool ready = false;

    private PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();

        foreach (Transform mainObject in MainGameObjects.transform)
        {
            EssentialGameObjects.Add(mainObject.gameObject);
        }

        if (MainGameObjects == null)
        {
            ready = true;
        }
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartObjects()
    {
        MainGameObjects.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").SetActive(true);
        EssentialGameObjects.Find(a => a.gameObject.name == "Teleporting"); //todo: remove if not needed
        foreach (GameObject mainObject in EssentialGameObjects.ToArray())
        {
            //mainObject.SetActive(true);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Camera>().enabled = false;
        gameObject.GetComponent<AudioListener>().enabled = false;
        yield return new WaitUntil(() => ready);
        StartObjects();
    }
}
