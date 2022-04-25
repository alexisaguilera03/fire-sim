using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    private GameObject[] houses;

    private bool singleHouse;
    // Start is called before the first frame update
    void Start()
    {
        getUnloadedHouses();
        if (houses.Length == 1)
        {
            singleHouse = true;
        }
        StartCoroutine(loadHouses());
    }

    // Update is called once per frame
    void Update()
    {
        if (singleHouse)
        {
            singleHouse = false;
            StartCoroutine(loadHouse());
        }
        
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

    IEnumerator loadHouse()
    {
        yield return new WaitForEndOfFrame();
        houses[0].SetActive(true);
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
