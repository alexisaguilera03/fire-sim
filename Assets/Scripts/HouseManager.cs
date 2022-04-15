using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    private GameObject[] houses;
    // Start is called before the first frame update
    void Start()
    {
        getUnloadedHouses();
        StartCoroutine(loadHouses());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void getUnloadedHouses()
    {
        List<GameObject> tmp = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                tmp.Add(child.gameObject);
            }
            
        }
        houses = tmp.ToArray();
    }

    IEnumerator loadHouses()
    {
        yield return new WaitForSeconds(10);
        foreach (GameObject house in houses)
        {
            house.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }
}
