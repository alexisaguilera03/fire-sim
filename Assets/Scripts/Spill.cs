using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Spill : MonoBehaviour
{
    public GameObject ProjectileGameObject;
    public GameObject spawner;
    private ParticleSystem myParticleSystem;
    private Quaternion originalPosition;
    private bool isPlaying = false;
    private GameObject projectile;
    private bool ProjectileActive = false;
    private int index;
   

   // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        originalPosition = gameObject.transform.rotation;
        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i] == this)
            {
                index = i;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        myParticleSystem.transform.position = spawner.transform.position;
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.parent.transform.rotation = Quaternion.Euler(91,0,0);
        }
        if ((isPlaying && !ProjectileActive) || (projectile == null && ProjectileActive == true))
        {
            fire();
            StartCoroutine(waitProjectile(3));
        }

        if (gameObject.GetComponentInParent<Hand>() == null)
        {
            return;
        }
        float rotation = Mathf.Abs(gameObject.transform.parent.rotation.x);
        if(rotation > 0.71f && rotation != 0)
        {

            if (!isPlaying)
            {
                isPlaying = true;
                myParticleSystem.transform.position = gameObject.transform.position;
                myParticleSystem.Play();
                
            }
            
        }
        else
        {
            isPlaying = false;
            myParticleSystem.Stop();
        }
        
    }

    void fire()
    {

        ProjectileActive = true;
        projectile = Instantiate(ProjectileGameObject, spawner.transform.position, gameObject.transform.rotation);
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        projectile.GetComponent<MeshRenderer>().enabled = true; //delete when done
        projectile.GetComponent<Projectile>().shotFrom = gameObject;
        projectile.GetComponent<Projectile>().index = index;
        
    }


    
        IEnumerator waitProjectile(int seconds)
        {
            
            yield return new WaitForSeconds(seconds);
            ProjectileActive = false;
            Destroy(projectile);
        }

    }
