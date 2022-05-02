using UnityEngine;

public class DetectEscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        gameObject.transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, camera.transform.position.y, gameObject.transform.position.z), transform.rotation);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.transform.root.gameObject.CompareTag("Player")) return;
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().winCondition.Win = true;
        print("Escaped!");
        Destroy(gameObject);
    }
}
