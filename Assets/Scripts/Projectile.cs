using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Extinguisher extinguisher;
    public GameObject shotFrom;
    public int index;
    public AudioSource SuccessExtinguishAudioSource;
    public AudioSource GreaseFireDeathAudioSource;
    private SoundEngine soundEngine;
    private SceneManager sceneManager;

    private void Start()
    {
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }
    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (SuccessExtinguishAudioSource != null)
            {
                sceneManager.winCondition.WinAudioSource = SuccessExtinguishAudioSource;
            }
            collision.gameObject.GetComponent<Fire>().stopFire();
        }

        if (extinguisher != null)
        {
            extinguisher.projectileActive = false;
            extinguisher.StopAllCoroutines();
        }

        if (shotFrom != null)
        {
            if (shotFrom.gameObject.name != "Mug")
            {
                sceneManager.loseCondition.LoseAudioSource = GreaseFireDeathAudioSource;
                sceneManager.loseCondition.setLost(true);
            }
            MonoBehaviour[] scripts = shotFrom.GetComponents<MonoBehaviour>();
            scripts[index].StopAllCoroutines();
        }
        Destroy(gameObject);
    }

}
