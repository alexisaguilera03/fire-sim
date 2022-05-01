using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Menu, Kitchen, Escape, FireFighter, Credit, _kitchen, Teleporting, LoadingScreen;

    public static GameObject player;

    public string currentLevel = "", nextLevel = "";

    private GameObject current, next, loading;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentLevel = "Menu";
        nextLevel = "Kitchen";
        loading = Instantiate(LoadingScreen, new Vector3(0f, 300f, 0f), new Quaternion());
        loading.SetActive(false);
        StartCoroutine(getPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelect(string level)
    {
        nextLevel = level;
        Load();
    }

    public void LoadFirstLevel()
    {
        StartCoroutine(loadFirstLevel());
    }

    private IEnumerator loadFirstLevel()
    {
        if (current.name.Contains("Menu"))
        {
            current.SetActive(false);
        }
        loading.SetActive(true);
        yield return new WaitForSeconds(3);
        //todo: loading screen
        currentLevel = "Kitchen";
        loading.SetActive(false);
        player.SetActive(true);
        current = Instantiate(Kitchen);
        yield return new WaitForFixedUpdate();
        current.SetActive(true);

    }

    public void Load()
    {
        //todo: loading screen

        StartCoroutine(load());
    }

    private IEnumerator load()
    {
        current.SetActive(false);
        player.SetActive(false);
        loading.SetActive(true);
        yield return new WaitForSeconds(3);
        switch (nextLevel)
        {
            case "Kitchen":
                break;
            case "Escape":
                player.transform.position = new Vector3(22f, 2.282f, -28.68f);
                player.transform.rotation = Quaternion.Euler(0,0,0);
                currentLevel = "Escape";
                current = Instantiate(Escape);
                player.SetActive(true);
                yield return new WaitForEndOfFrame();
                current.SetActive(true);
                loading.SetActive(false);
                yield return new WaitForSeconds(7.5f);
                current.transform.Find("MainObjects").gameObject.SetActive(true);
                break;
            case "FireFighter":
                currentLevel = "FireFighter";
                current = Instantiate(FireFighter);
                current.transform.Find("HouseManager");
                var test = current.transform.Find("FireTruck").GetComponent<Firetruck>();
                var a = test.GetComponent<Camera>();
                player.SetActive(true);
                yield return new WaitForEndOfFrame();
                current.SetActive(true);
                loading.SetActive(false);
                break;
            default:
                throw new UnityException("Could not load next level");
        }
        
    }

    public void reset()
    {

    }

    IEnumerator getPlayer()
    {
        GameObject tmp = Instantiate(Kitchen);
        tmp.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitUntil(() => player.GetComponentInChildren<Fade>() != null);
        Kitchen = _kitchen;
        player.transform.parent = null;
        Teleporting.SetActive(true);
        player.SetActive(false);
        yield return new WaitForFixedUpdate();
        tmp.BroadcastMessage("StopAllCoroutines");
        tmp.SetActive(false);
        current = Instantiate(Menu);
        current.SetActive(true);

    }
}
