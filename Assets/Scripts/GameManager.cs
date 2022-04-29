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

    public GameObject Menu, Kitchen, Escape, FireFighter, Credit, _kitchen, Teleporting;

    public static GameObject player;

    public string currentLevel = "", nextLevel = "";

    private GameObject current, next;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentLevel = "Menu";
        nextLevel = "Kitchen";
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

        yield return new WaitForFixedUpdate();
        //todo: loading screen
        currentLevel = "Kitchen";
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
        yield return new WaitForFixedUpdate();
        switch (nextLevel)
        {
            case "Kitchen":
                break;
            case "Escape":
                currentLevel = "Escape";
                current = Instantiate(Escape);
                player.SetActive(true);
                yield return new WaitForEndOfFrame();
                current.SetActive(true);
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
