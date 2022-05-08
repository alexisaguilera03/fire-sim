using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Pickup))]
public class Hose : MonoBehaviour
{
    

    public GameObject particles = null;
    public GameObject projectile = null;
    public GameObject barrel = null;
    public float Force = 5.5f;


    private bool isAttached = false;
    public bool projectileActive = false;
    private bool particlesActive = false;
    private bool displayHint = true;

    private SteamVR_Action_Boolean grabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    private SteamVR_Action_Boolean releaseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    private Hand AttachedHand = null;

    private Interaction interactionSystem = null;

    private SoundEngine soundEngine = null;

    private GameObject firedProjectile;

    private HintSystem hintSystem;

    private AudioSource waterAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        interactionSystem = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponent<Interaction>();
        soundEngine = GameObject.FindGameObjectWithTag("SoundEngine").GetComponent<SoundEngine>();
        hintSystem = GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>();
    }

    // Update is called once per frame
    void Update()
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
            hintSystem.displayHint(HintSystem.Hint.Hose);
            displayHint = false;
        }

        AttachedHand ??= gameObject.transform.parent.gameObject.GetComponent(typeof(Hand)) as Hand;
        if (grabAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!particlesActive)
            {
                particles.gameObject.SetActive(true);
                particlesActive = true;
                Fire();
            }
            else
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
                StopAllCoroutines();
                Destroy(firedProjectile);
            }
        }


        if (releaseAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(AttachedHand, gameObject, ref isAttached);
            AttachedHand = null;
            isAttached = false;
            if (firedProjectile != null)
            {
                StopAllCoroutines();
                Destroy(firedProjectile);
            }

            if (particles.gameObject.activeSelf)
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
            }
        }
    }

    void LateUpdate()
        {
            isAttached = interactionSystem.checkAttached(gameObject);
        }

        void Fire()
        {
            if (hintSystem.activeHint == HintSystem.Hint.Hose)
            {
                hintSystem.hintTaken = true;
            }
            firedProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation) as GameObject;
            Physics.IgnoreCollision(firedProjectile.GetComponent<Collider>(), barrel.GetComponentInChildren<Collider>());
            Physics.IgnoreCollision(firedProjectile.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i] == this)
                {
                    firedProjectile.GetComponent<Projectile>().index = i;
                }
            }
            projectile.GetComponent<Projectile>().shotFrom = gameObject;
            projectileActive = true;
            firedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Force, ForceMode.Impulse);
            //todo: find rushing water sound effect
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

