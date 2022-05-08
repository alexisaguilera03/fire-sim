using System.Collections;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]

public class Extinguisher : MonoBehaviour
{
    private SteamVR_Action_Boolean grabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    private SteamVR_Action_Boolean releaseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public GameObject particles = null;
    public GameObject projectile = null;
    public GameObject barrel = null;
    public float Force = 5.5f;
    

    private bool isAttached = false;
    public bool projectileActive = false;
    private bool particlesActive = false;
    private bool displayHint = true;

    // private bool m_IsReloading = false;
    private Hand AttachedHand = null;
    private Interaction interactionSystem = null;
    private SoundEngine soundEngine = null;
    private GameObject firedProjectile;
    private AudioSource ExtinguisherAudioSource;
    private AudioSource successExtinguisherAudioSource;
    private HintSystem hintSystem;



    private void Start()
    {
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        if (audioSources.Length == 0)
        {
            Debug.LogError("Extinguisher AudioSource must be set");
        }
        ExtinguisherAudioSource = audioSources[0];
        ExtinguisherAudioSource = audioSources.ToList().Find(x => x.clip.name == "Fire Extinguisher");
        successExtinguisherAudioSource = (audioSources.Length > 1) ? audioSources[1] : null;
        interactionSystem = GetComponentInParent<Interaction>();
        if (interactionSystem == null)
        {
            interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        }

        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        hintSystem = GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>();
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

        if (displayHint)
        {
            hintSystem.displayHint(HintSystem.Hint.Extinguisher);
            displayHint = false;
        }
        AttachedHand ??= gameObject.transform.parent.gameObject.GetComponent(typeof(Hand)) as Hand;
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
        if (hintSystem.activeHint == HintSystem.Hint.Extinguisher)
        {
            hintSystem.hintTaken = true;
        }
        firedProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation) as GameObject;
        Physics.IgnoreCollision(firedProjectile.GetComponent<Collider>(), gameObject.GetComponentInChildren<Collider>());
        firedProjectile.GetComponent<Projectile>().extinguisher = this;
        if (GameManager.Instance.currentLevel != "FireFighter")
        {
            firedProjectile.GetComponent<Projectile>().SuccessExtinguishAudioSource = successExtinguisherAudioSource;
        }
        projectileActive = true;
        firedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * Force, ForceMode.Impulse);
        soundEngine.PlaySoundEffect(ExtinguisherAudioSource, true, false);
        StartCoroutine(waitProjectile(2));
    }

     IEnumerator waitProjectile(int seconds)
     {
         yield return new WaitForSeconds(seconds);
         projectileActive = false;
         Destroy(firedProjectile);
     }


}

   

