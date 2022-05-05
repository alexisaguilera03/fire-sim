using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Firetruck : MonoBehaviour
{

    public GameObject TargetPositionGameObject, FireExtinguisher, Mask, Houses, House, Nozzle;

    public Camera followCamera;

    public  AudioSource siren, WinAudioSource;
    private SoundEngine soundEngine;

    private bool ready = false, rotate = false;

    public bool test;

    // Start is called before the first frame update
    void Start()
    {
        followCamera = transform.GetChild(0).GetComponent<Camera>();
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        TargetPositionGameObject.transform.SetParent(transform, true);
        StartCoroutine(Drive());
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            checkHouses();
        }

        if (rotate)
        {
            followCamera.gameObject.transform.rotation = Quaternion.Slerp(followCamera.gameObject.transform.rotation, Quaternion.LookRotation(House.transform.position - followCamera.gameObject.transform.position), Time.deltaTime * 2);

        }

        if (test)
        {
            test = false;
            GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>().ExtinguishAllFires();
        }

        if (GameObject.FindGameObjectWithTag("SceneManager") == null) return;

        if (GameObject.FindGameObjectWithTag("SceneManager").GetComponent<WinCondition>().WinAudioSource == null)
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<WinCondition>().WinAudioSource = WinAudioSource;
        }
    }

    void checkHouses()
    {
        foreach (Transform house in Houses.transform)
        {
            if (!house.gameObject.activeSelf)
            {
                return;
            }
        }

        ready = true;
    }

    void Play()
    {
        GameObject player = (Player.instance == null) ? Resources.FindObjectsOfTypeAll<Player>()[0].gameObject : Player.instance.rigSteamVR;
        player.SetActive(true);
        gameObject.transform.position = new Vector3(transform.position.x, 1.479f, transform.position.z);
        Load.ready = true;
    }

    IEnumerator Drive()
    {
        //todo: add siren sound effect
        yield return new WaitUntil(() => ready);
        soundEngine.PlaySoundEffect(siren, true, false);
        followCamera.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        
        Vector3 targetPosition = TargetPositionGameObject.transform.position;
        float distanceFromHouse = Vector3.Distance(transform.position, TargetPositionGameObject.transform.position);
        float speed = 10f;
        while (transform.position != targetPosition)
        {
            distanceFromHouse = Vector3.Distance(transform.position, House.transform.position);
            if (distanceFromHouse <= 55f)
            {
                if (speed >= 3 && distanceFromHouse <= 22.50f)
                {
                    speed -= .1f;
                }
                
                rotate = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
        
        yield return new WaitForSeconds(2);
        soundEngine.StopSound(siren);
        rotate = false;
        Destroy(followCamera.gameObject);
        //Play();

    }
}
