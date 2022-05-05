using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = System.Random;

public class FireManager : MonoBehaviour
{
    public int fireCount = 0;
    public GameObject FireGameObject;
    private Random rng;
    private bool Spread = true;
    private SoundEngine soundEngine;
    private AudioSource FireAudioSource;
    private const int maxFires = 50; 
    // Start is called before the first frame update
    void Start()
    {
        rng = new Random((int) DateTime.Now.Ticks);
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        FireAudioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(wait(15));
    }

    // Update is called once per frame
    void Update()
    {
        if (soundEngine.fireActive && fireCount <= 0)
        {
            soundEngine.StopSound(FireAudioSource);
            soundEngine.fireActive = false;
        }

    }

    

    public void spread(Vector3 center)
    {
        if (fireCount >= maxFires) return;
        if (Spread) return; 
        Collider[] hitColliders = Physics.OverlapSphere(center, 5f);
        List <Collider> tmp = new List<Collider>();
        tmp = hitColliders.ToList();
        tmp.RemoveAll(x => x.GetComponentInParent<HouseManager>() != null);
        tmp.RemoveAll(x => x.GetComponentInParent<Player>()!= null);
        tmp.RemoveAll(x => x.GetComponentInParent<Fire>()!= null || x.GetComponent<Fire>() != null);
        tmp.RemoveAll(x => x.gameObject.transform.root.CompareTag("Extinguisher"));
        tmp.RemoveAll(x => x.GetComponentInParent<Interaction>() != null);
        tmp.RemoveAll(x => x.GetComponentInParent<ZoneManager>()!= null || x.GetComponent<ZoneManager>()!= null);
        tmp.RemoveAll(x => x.gameObject.name.Contains("Cube"));
        hitColliders = tmp.ToArray();
        GameObject newObject = hitColliders[rng.Next(0, hitColliders.Length)].gameObject;
        createFire(newObject.transform.position, FireGameObject.transform.rotation);
        StartCoroutine(wait());
    }
    private IEnumerator wait()
    {
        Spread = true;
        yield return new WaitForSeconds(rng.Next(5, 10));
        Spread = false;
    }

    private IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Spread = false; 
    }

    public void createFire(Transform newTransform)
    {
        if (newTransform.gameObject.GetComponentInChildren<Fire>() != null)
        {
            return;
        } 
        if (fireCount >= maxFires) return;
        Instantiate(FireGameObject, newTransform.position, FireGameObject.transform.rotation, newTransform);
    }
    public void createFire(Vector3 position, Quaternion rotation)
    {
        if (fireCount >= maxFires) return;
        Instantiate(FireGameObject, position, rotation, gameObject.transform);
        fireCount++;
    }

    public GameObject createFireGameObject(Vector3 position, Quaternion rotation)
    {
        if (fireCount >= maxFires) return null;
        fireCount++;
        return Instantiate(FireGameObject, position, rotation, transform);
    }

    public void ExtinguishAllFires()
    {
        Fire[] fires = GameObject.FindObjectsOfType<Fire>();
        StopAllCoroutines();
        foreach (Fire fire in fires)
        {
            fire.stopFire();
        }
    }
}
