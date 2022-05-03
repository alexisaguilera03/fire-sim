using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Menu, Kitchen, Escape, FireFighter, Credit, _kitchen, Teleporting, LoadingScreen;

    public AudioSource FireAlarm;

    public static GameObject player;

    public string currentLevel = "", nextLevel = "";

    private GameObject current, next, loading, Camera;

    private Fade fader;

    private List<GameObject> gc = new List<GameObject>();

    private bool isLoading;

    public bool isPlaying;


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
        isPlaying = FireAlarm.isPlaying;
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
            gc.Add(current);
            current.SetActive(false);
        }
        loading.SetActive(true);
        yield return new WaitForSeconds(3);
        //todo: loading screen
        currentLevel = "Kitchen";
        loading.SetActive(false);
        Camera.SetActive(true);
        current = Instantiate(Kitchen);
        yield return new WaitForFixedUpdate();
        current.SetActive(true);
        StartCoroutine(garbageCollect());

    }

    public void Load()
    {

        if (isLoading) return;
        StartCoroutine(load());
    }

    private IEnumerator load()
    {
        if (player.GetComponent<SteamVR_LaserPointer>() != null)
        {
            Destroy(player.GetComponent<SteamVRLaserWrapper>());
            Destroy(player.GetComponent<SteamVR_LaserPointer>());
        }
        isLoading = true;
        gc.Add(current);
        if (FireAlarm.isPlaying)
        {
            FireAlarm.Stop();
        }
        fader.FadeIn(Color.black, 1);
        yield return new WaitForSeconds(1);
        current.SetActive(false);
        Camera.SetActive(false);
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
                current.SetActive(true);
                loading.SetActive(false);
                FireAlarm.loop = true;
                FireAlarm.Play();
                var tmp = current.transform.Find("PlayerManager");
                yield return new WaitUntil(() => tmp.GetComponent<Camera>() != null);
                yield return new WaitUntil(() => tmp.GetComponent<Camera>() == null);
                FireAlarm.volume /= 3;
                Camera.SetActive(true);
                current.transform.Find("MainObjects").gameObject.SetActive(true);
                break;
            case "FireFighter":
                player.transform.position = new Vector3(21.71f, 0f, -47.9f);
                currentLevel = "FireFighter";
                current = Instantiate(FireFighter);
                current.transform.Find("HouseManager");
                var test = current.transform.Find("FireTruck").GetComponent<Firetruck>();
                current.SetActive(true);
                loading.SetActive(false);
                yield return new WaitUntil(() => current.transform.Find("FireTruck").GetComponent<Firetruck>().followCamera != null);
                yield return new WaitUntil(() => current.transform.Find("FireTruck").GetComponent<Firetruck>().followCamera == null);
                current.transform.Find("MainObjects").gameObject.SetActive(true);
                Camera.SetActive(true);
                break;
            case "Credits":
                currentLevel = "Credits";
                current = Instantiate(Credit);
                current.SetActive(true);
                loading.SetActive(false);
                yield return new WaitForFixedUpdate();
                Camera.SetActive(true);
                break;
            default:
                throw new UnityException("Could not load next level");
        }

        isLoading = false;
        StartCoroutine(garbageCollect());
    }

    public void reset()
    {
        nextLevel = currentLevel;
        var tmp = player.GetComponentsInChildren<Fire>();
        if (tmp.Length > 0)
        {
            foreach (var t in tmp)
            {
                t.stopFire();
            }
        }

        StartCoroutine(load());
    }

    IEnumerator getPlayer()
    {
        GameObject tmp = Instantiate(Kitchen);
        tmp.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        player.AddComponent<SteamVRLaserWrapper>();
        player.AddComponent<SteamVR_LaserPointer>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        yield return new WaitUntil(() => player.GetComponentInChildren<Fade>() != null);
        fader = player.GetComponentInChildren<Fade>();
        Kitchen = _kitchen;
        player.transform.parent = null;
        Teleporting.SetActive(true);
        Camera.SetActive(false);
        yield return new WaitForFixedUpdate();
        tmp.BroadcastMessage("StopAllCoroutines");
        tmp.SetActive(false);
        gc.Add(tmp);
        current = Instantiate(Menu);
        current.SetActive(true);

    }

    IEnumerator garbageCollect()
    {
        yield return new WaitForSeconds(5f);
        if (gc.Count <= 0) yield break;
        var tmp = gc[0];
        gc.RemoveAt(0);
        Destroy(tmp);

    }
}
