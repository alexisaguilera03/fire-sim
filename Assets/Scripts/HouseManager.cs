using System.Collections;
using System.Linq;
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

        if (GameManager.Instance.currentLevel == "FireFighter")
        {
            StartCoroutine(loadHouses());
        }
        StartCoroutine(loadHouses(10));
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
        houses = (from Transform child in transform where !child.gameObject.activeSelf select child.gameObject).ToArray();
    }

    IEnumerator loadHouse()
    {
        yield return new WaitForEndOfFrame();
        houses[0].SetActive(true);
    }

    IEnumerator loadHouses(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (GameObject house in houses)
        {
            house.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator loadHouses()
    {
        yield return new WaitForFixedUpdate();
        foreach (GameObject house in houses)
        {
            house.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
