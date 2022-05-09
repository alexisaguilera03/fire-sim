using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{

    public Extinguisher extinguisher;
    public GameObject shotFrom;
    public int index;
    public AudioSource SuccessExtinguishAudioSource;
    public AudioSource GreaseFireDeathAudioSource;
    private SoundEngine soundEngine;
    private SceneManager sceneManager;
    private bool exploded;

    private void Start()
    {
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }
    private void Update()
    {
        
    }

    private AudioSource returnAudioSource()
    {
        var test = new AudioSource();
        return GreaseFireDeathAudioSource;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fire") && !exploded)
        {
            if (SuccessExtinguishAudioSource != null)
            {
                sceneManager.winCondition.WinAudioSource = SuccessExtinguishAudioSource;
            }
            if (shotFrom != null && shotFrom.gameObject.name == "Mug")
            {
                exploded = true;
                bool attached = false;
                var interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
                interactionSystem.Release(shotFrom.GetComponentInParent<Hand>(), shotFrom, ref attached);
                //sceneManager.loseCondition.LoseAudioSource = returnAudioSource();
                sceneManager.loseCondition.setLost(true);
                MonoBehaviour[] scripts = shotFrom.GetComponents<MonoBehaviour>();
                if (scripts.Length == 1)
                {
                    scripts[0].StopAllCoroutines();
                }
                else
                {
                    scripts[index].StopAllCoroutines();
                }
            }
            else
            {
                collision.gameObject.GetComponent<Fire>().stopFire();
            }
            
            
        }

        if (extinguisher != null)
        {
            extinguisher.projectileActive = false;
            extinguisher.StopAllCoroutines();
        }

        if (shotFrom != null)
        {
            MonoBehaviour[] scripts = shotFrom.GetComponents<MonoBehaviour>();
            if (scripts.Length == 1)
            {
                scripts[0].StopAllCoroutines();
            }
            else
            {
                scripts[index].StopAllCoroutines();
            }
            
        }
        Destroy(gameObject);
    }

}
