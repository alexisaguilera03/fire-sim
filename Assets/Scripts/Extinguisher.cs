using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]

public class Extinguisher : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction = null;
    public SteamVR_Action_Boolean releaseAction = null;
    public Text m_AmmoOutput = null;
    public GameObject particles = null;
    public GameObject projectile = null;
    public GameObject barrel = null;
    public float Force = 5.5f;
    

    private bool isAttached = false;
    private bool softAttach = false; //soft attach being true means the player has to hold the trigger to keep item in hand
    public bool projectileActive = false;
    private bool particlesActive = false;

    // private bool m_IsReloading = false;
    private Hand AttachedHand = null;
    private Interaction interactionSystem = null;
    private SoundEngine soundEngine = null;
    private Pickup pickup = null;
    private GameObject firedProjectile;
    private AudioSource ExtinguisherAudioSource;
    private AudioSource successExtinguisherAudioSource;

    private Rigidbody projectileRigidBody = null;

    private void Awake()
    {
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        if (audioSources.Length == 0)
        {
            Debug.LogError("Extinguisher AudioSource must be set");
        }
        ExtinguisherAudioSource = audioSources[0];
        successExtinguisherAudioSource = (audioSources.Length > 1) ? audioSources[1] : null;
        pickup = gameObject.GetComponent(typeof(Pickup)) as Pickup;
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
    }

    private void Update()
    {
        if (particlesActive && firedProjectile == null)
        {
            Fire();
        }

        if (!isAttached)
        {
            return;
        }

        if (AttachedHand is null)
        {
            AttachedHand = gameObject.transform.parent.gameObject.GetComponent(typeof(Hand)) as Hand;
        }
        //print(SteamVR_Actions._default.Squeeze.GetAxis(AttachedHand.handType));
        if (grabAction.GetStateDown(AttachedHand.handType)|| Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(!particlesActive)
            {
                particles.gameObject.SetActive(true);
                particlesActive = true;
                Fire();
            }
            else
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
                soundEngine.StopSound(ExtinguisherAudioSource);
                StopAllCoroutines();
                Destroy(firedProjectile);
            }
        }
       

        if (releaseAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(AttachedHand, gameObject, ref isAttached);
            AttachedHand = null;
            isAttached = false;
            if(firedProjectile != null)
            {
                StopAllCoroutines();
                Destroy(firedProjectile);
            }
            if (particles.gameObject.activeSelf)
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
                soundEngine.StopSound(ExtinguisherAudioSource);
            }
        }
    }

    void LateUpdate()
    {
        isAttached = interactionSystem.checkAttached(gameObject);
    }

    void Fire()
    {
        firedProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation) as GameObject;
        firedProjectile.GetComponent<Projectile>().extinguisher = this;
        firedProjectile.GetComponent<Projectile>().SuccessExtinguishAudioSource = successExtinguisherAudioSource;
        projectileActive = true;
        firedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * Force, ForceMode.Impulse);
        //soundEngine.PlaySoundEffect(ExtinguisherAudioSource, true, false);
        StartCoroutine(waitProjectile(2));
    }

     IEnumerator waitProjectile(int seconds)
     {
         yield return new WaitForSeconds(seconds);
         projectileActive = false;
         Destroy(firedProjectile);
     }


}

   

