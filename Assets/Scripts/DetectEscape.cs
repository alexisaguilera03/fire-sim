using UnityEngine;

public class DetectEscape : MonoBehaviour
{
    public AudioSource escapeAudio;

    private bool escaped;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        gameObject.transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, camera.transform.position.y, gameObject.transform.position.z), transform.rotation);
        if (GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().winCondition.WinAudioSource == null)
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().winCondition.WinAudioSource = escapeAudio;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.transform.root.gameObject.CompareTag("Player")) return;
        if (escaped) return;
        escaped = true;
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().winCondition.Win = true;
        print("Escaped!");
        //Destroy(gameObject);
    }
}
