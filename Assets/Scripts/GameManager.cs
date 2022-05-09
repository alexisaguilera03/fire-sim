using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Menu, Kitchen, Escape, FireFighter, Credit, _kitchen, Teleporting, LoadingScreen;

    public AudioSource FireAlarm, KitchenTimeout, HandsOnFire, EscapeTimeout, FireFighterTimeout, KitchenIntro, EscapeIntro, FireFighterIntro;

    public static GameObject player;

    public string currentLevel = "", nextLevel = "";

    private GameObject current, next, loading, Camera;

    private Fade fader;

    private List<GameObject> gc = new List<GameObject>();

    private bool isLoading, isResetting;

    public bool isPlaying, TimedOut, handsOnFire;


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
        fader.FadeIn(Color.white, 2f);
        yield return new WaitForSeconds(2.1f);
        gc.Add(current);
        current.SetActive(false);
        
        if (player.GetComponentInChildren<SteamVR_LaserPointer>() != null)
        {
            Destroy(player.GetComponentInChildren<SteamVRLaserWrapper>()); 
            Destroy(player.GetComponentInChildren<SteamVR_LaserPointer>());
            Destroy(player.GetComponent<Player>().rightHand.transform.Find("New Game Object").gameObject);
        }
        loading.SetActive(true);
        yield return new WaitForSeconds(3);
        currentLevel = "Kitchen";
        current = Instantiate(Kitchen);
        yield return new WaitForFixedUpdate();
        player.transform.position = new Vector3(-3.09f, 0.42f, -5.15f);
        player.transform.rotation = Quaternion.Euler(0,180,0);
        loading.SetActive(false);
        current.SetActive(true);
        KitchenIntro.playOnAwake = true;
        KitchenIntro.enabled = true;
        Camera.SetActive(true);
        fader.FadeOut(1);
        StartCoroutine(garbageCollect());

    }

    public void Load()
    {

        if (isLoading) return;
        StartCoroutine(load());
    }

    private IEnumerator load()
    {
        isLoading = true;
        if (player.GetComponentInChildren<SteamVR_LaserPointer>() != null)
        {
            Destroy(player.GetComponentInChildren<SteamVRLaserWrapper>());
            Destroy(player.GetComponentInChildren<SteamVR_LaserPointer>());
            Destroy(player.GetComponent<Player>().rightHand.transform.Find("New Game Object").gameObject);
        }
        
        gc.Add(current);
        if (FireAlarm.isPlaying)
        {
            FireAlarm.Stop();
        }

        if (TimedOut && KitchenTimeout != null && !handsOnFire)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                source.Stop();
            }
            
            switch (currentLevel)
            {
                case "Kitchen":
                    KitchenTimeout.Play();
                    yield return new WaitUntil(() => !KitchenTimeout.isPlaying);
                    break;
                case "Escape":
                    EscapeTimeout.Play();
                    yield return new WaitUntil(() => !EscapeTimeout.isPlaying);
                    break;
                case "FireFighter":
                    FireFighterTimeout.Play();
                    yield return new WaitUntil(() => !FireFighterTimeout.isPlaying);
                    break;
            }
        }

        if (handsOnFire)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                source.Stop();
            }
            HandsOnFire.Play();
            yield return new WaitUntil(() => !HandsOnFire.isPlaying);
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
                StartCoroutine(loadFirstLevel());
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
                EscapeIntro.playOnAwake = true;
                EscapeIntro.enabled = true;
                yield return new WaitUntil(() => tmp.GetComponent<Camera>() == null);
                FireAlarm.volume /= 3;
                Camera.SetActive(true);
                current.transform.Find("MainObjects").gameObject.SetActive(true);
                break;
            case "FireFighter":
                player.transform.position = new Vector3(21.71f, 1f, -47.9f);
                currentLevel = "FireFighter";
                current = Instantiate(FireFighter);
                current.SetActive(true);
                yield return new WaitUntil(() => current.transform.Find("FireTruck").GetComponent<Firetruck>().followCamera != null);
                loading.SetActive(false);
                yield return new WaitUntil(() => current.transform.Find("FireTruck").GetComponent<Firetruck>().followCamera == null);
                FireFighterIntro.playOnAwake = true;
                FireFighterIntro.enabled = true;
                current.transform.Find("MainObjects").gameObject.SetActive(true);
                Camera.SetActive(true);
                break;
            case "Credits":
                currentLevel = "Credits";
                current = Instantiate(Credit);
                player.transform.position = new Vector3(-7.1f, 1f, -5.34f);
                player.transform.rotation = Quaternion.Euler(0,0,0);
                loading.SetActive(false);
                current.SetActive(true);
                yield return new WaitForFixedUpdate();
                Camera.SetActive(true);
                break;
            default:
                throw new UnityException("Could not load next level");
        }
        isLoading = false;
        isResetting = false;
        StartCoroutine(garbageCollect());
    }

    public void reset()
    {
        if (isResetting) return;
        isResetting = true;
        fader.FadeIn(Color.white, 1);
        nextLevel = currentLevel;
        var tmp = player.GetComponentsInChildren<Fire>();
        if (tmp.Length > 0)
        {
            foreach (var t in tmp)
            {
                t.stopFire();
            }

            if (player.GetComponent<Player>().rightHand.gameObject.GetComponent<Collider>().enabled == false || player.GetComponent<Player>().leftHand.gameObject.GetComponent<Collider>().enabled == false)
            {
                var _player = player.GetComponent<Player>();
                var left = _player.leftHand.gameObject;
                var right = _player.rightHand.gameObject;
                left.GetComponent<Collider>().enabled = true;
                right.GetComponent<Collider>().enabled = true;


            }
        }
        

        StartCoroutine(TimedOut ? playTimeout() : load());
    }

    IEnumerator playTimeout()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
        switch (currentLevel)
        {
            case "Kitchen":
                KitchenTimeout.Play();
                yield return new WaitUntil(() => !KitchenTimeout.isPlaying);
                break;
            case "Escape":
                EscapeTimeout.Play();
                yield return new WaitUntil(() => !EscapeTimeout.isPlaying);
                break;
            case "FireFighter":
                FireFighterTimeout.Play();
                yield return new WaitUntil(() => !FireFighterTimeout.isPlaying);
                break;
            default:
                yield break;
        }

        StartCoroutine(load());
    }
    IEnumerator getPlayer()
    {
        GameObject tmp = Instantiate(Kitchen);
        tmp.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        yield return new WaitUntil(() => player.GetComponentInChildren<Fade>() != null);
        player.transform.position = new Vector3(-0.42f, 0.692f, -2.026f);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
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
        Camera.SetActive(true);

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
