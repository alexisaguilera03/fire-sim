using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    private GameObject fire;

    private FireManager fireManager;

    private bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        fireManager = GameObject.FindGameObjectWithTag("FireManager").GetComponent<FireManager>();
        gameObject.GetComponent<Renderer>().enabled = false;
        initialized = true;
        fire ??= fireManager.createFireGameObject(gameObject.transform.position, gameObject.transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if (!initialized) return;
        fire ??= fireManager.createFireGameObject(gameObject.transform.position, gameObject.transform.rotation);
    }

    void OnDisable()
    {
        
    }
}
