using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        startZoneOnStart();
        if (soundEngine.fireActive) return;

    }

    // Update is called once per frame
    void Update()
    {
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

    void OnEnable()
    {
        Start(); //is this line needed?
    }

    void OnDisable()
    {
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
            ceilingSmokeGameObjects.Add(smokeDetection.gameObject);
        }

        if (ceilingSmokeGameObjects.Count == 0)
        {
            throw new UnityException("Could not get Ceiling Smoke Objects from Children");
        }
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
