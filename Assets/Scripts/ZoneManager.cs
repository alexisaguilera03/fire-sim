using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Valve.VR.InteractionSystem;

public class ZoneManager : MonoBehaviour
{
    [Header("Zone Options")]
    public string zoneName = "";
    public bool ceilingSmokeEnabled = false;
    [Tooltip("True if you want to use the players vertical distance to disable zone")]
    public bool usePlayerHeight = false;

    public GameObject nextZone;
    public GameObject previousZone;
    [Header("")]
    public GameObject[] fireSpawners;

    [Tooltip("Other game objects that need set active")]
    public GameObject[] Objects;
    public GameObject[] ceilingSmoke;


    private bool active = false;
    private bool nextZoneTriggered = false;
    private bool zoneActiveByDefault;
    private GameObject player;

    private SoundEngine soundEngine;


    // Start is called before the first frame update
    void Start()
    {
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectsWithTag("Zone").Length  % 2 != 0 && nextZone == null)
        {
            throw new UnityException("Next Zone Trigger needs to be assigned in inspector");
        }

        gameObject.name = (zoneName == "") ? gameObject.name : zoneName;
        if (fireSpawners.Length == 0)
        {
           tryGetFireSpawnersInChildren();
        }

        if (ceilingSmoke.Length == 0 && ceilingSmokeEnabled)
        {
            tryGetCeilingSmokeInChildren();
        }
        getOtherObjects();
        gameObject.transform.DetachChildren(); //prevent objects from following player and is super janky but works
        startZoneOnStart();
        StartCoroutine(wait());

    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, camera.transform.position.y, gameObject.transform.position.z);
        if (usePlayerHeight)
        {
            float playerHeight = player.transform.localPosition.y;
            if (playerHeight < gameObject.transform.localPosition.y)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void resetParent()
    {
        foreach (GameObject fireSpawner in fireSpawners)
        {
            fireSpawner.transform.SetParent(gameObject.transform, true);
        }

        if (ceilingSmokeEnabled)
        {
            foreach (GameObject smoke in ceilingSmoke)
            {
                smoke.transform.SetParent(gameObject.transform, true);
            }
        }

        if (Objects.Length > 0)
        {
            foreach (GameObject otherGameObject in Objects)
            {
                otherGameObject.transform.SetParent(gameObject.transform, true);
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        active = true;
    }

    void OnEnable()
    {
        Start(); //is this line needed?
    }

    void OnDisable()
    {
        resetParent();
        stopZone(); //is this function needed? Does every child become inactive if parent is inactive
    }

    void tryGetFireSpawnersInChildren()
    {
        var tmp = gameObject.GetComponentsInChildren<FireSpawner>(true);
        if (tmp.Length == 0)
        {
            throw new UnityException("Fire Spawners were not found in children and not set in inspector");
        }
            
        List<GameObject> fireSpawnersList = new List<GameObject>();
        foreach (FireSpawner fireSpawner in tmp)
        {
            fireSpawner.transform.SetParent(transform, true);
            fireSpawnersList.Add(fireSpawner.gameObject);
        }

        if (fireSpawnersList.Count == 0)
        {
            throw new UnityException("Could not get Fire Spawners from Children");
        }

        fireSpawners = fireSpawnersList.ToArray();
    }

    void tryGetCeilingSmokeInChildren()
    {
        var tmp = gameObject.GetComponentsInChildren<SmokeDetection>(true);
        if (tmp.Length == 0)
        {
            throw new UnityException("Ceiling Smoke Objects were not found in children and not set in inspector");
        }
        List<GameObject> ceilingSmokeGameObjects = new List<GameObject>();
        foreach (SmokeDetection smokeDetection in tmp)
        {
            smokeDetection.gameObject.transform.SetParent(gameObject.transform, true);
            ceilingSmokeGameObjects.Add(smokeDetection.gameObject);
        }

        if (ceilingSmokeGameObjects.Count == 0)
        {
            throw new UnityException("Could not get Ceiling Smoke Objects from Children");
        }

        ceilingSmoke = ceilingSmokeGameObjects.ToArray();
    }

    void getOtherObjects()
    {
        List<GameObject> tmp = new List<GameObject>();
        foreach (Transform child in transform)
        {
            tmp.Add(child.gameObject);
        }

        tmp = tmp.Except(fireSpawners.ToList()).ToList();
        tmp = tmp.Except(ceilingSmoke.ToList()).ToList();
        Objects = tmp.ToArray();

    }

    void startZoneOnStart()
    {
        foreach (GameObject fireSpanwer in fireSpawners)
        {
            fireSpanwer.SetActive(true);
        }

        if (ceilingSmokeEnabled)
        {
            foreach (GameObject smoke in ceilingSmoke)
            {
                smoke.SetActive(true);
            }
        }

        if (Objects.Length > 0)
        {
            foreach (GameObject gameObject in Objects)
            {
                gameObject.SetActive(true);
            }
        }
    }
    void startNextZone()
    {
        nextZone.GetComponent<ZoneManager>().previousZone = this.gameObject;
        nextZone.SetActive(true);
    }

    void stopZone()
    {
        foreach (GameObject fireSpanwer in fireSpawners)
        {
            fireSpanwer.SetActive(false);
        }

        if (ceilingSmokeEnabled)
        {
            foreach (GameObject smoke in ceilingSmoke)
            {
                smoke.SetActive(false);
            }
        }

        if (Objects.Length > 0)
        {
            foreach (GameObject gameObject in Objects)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.root.CompareTag("Player"))
        {
            if (nextZone != null)
            {
                startNextZone();
                nextZoneTriggered = true;
            }

            if (previousZone != null && !usePlayerHeight)
            {
                previousZone.SetActive(false);
            }
        }
    }

}
