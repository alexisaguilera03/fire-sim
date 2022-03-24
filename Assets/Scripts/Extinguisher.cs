using System.Collections;
using System.Collections.Generic;
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

    public float Force = 0.5f;

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

    private Rigidbody projectileRigidBody = null;

    private void Awake()
    {
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
    }
    private void Start()
    {
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
        if (projectileActive && particlesActive)
        {
           // firedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * Force, ForceMode.Impulse);
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
                Destroy(firedProjectile);
            }
        }
       

        if (releaseAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(AttachedHand, gameObject, ref isAttached);
            AttachedHand = null;
            isAttached = false;
        }

    }

     void LateUpdate()
    {
        isAttached = interactionSystem.checkAttached(gameObject);
    }

     void Fire()
    {
        firedProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation) as GameObject;
        firedProjectile.GetComponent<MeshRenderer>().enabled = true; //delete this line after done
        projectileActive = true;
        firedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * Force, ForceMode.Impulse);
        StartCoroutine(waitProjectile(2));
    }

     IEnumerator waitProjectile(int seconds)
     {
         yield return new WaitForSeconds(seconds);
         projectileActive = false;
         Destroy(firedProjectile);
     }


   // void HandHoverUpdate(Hand hand)
    //{
     //  interactionSystem.OnHandHover(hand, gameObject, ref isAttached);
      // if (!isAttached) return;
       //AttachedHand = hand;
       //softAttach = false;
    //}

    public void SetInnactive()
    {
        projectileRigidBody.velocity = Vector3.zero;
        projectileRigidBody.angularVelocity = Vector3.zero;
        projectile.gameObject.SetActive(false);
    }
}

/*  private void Fire()
  {
      if(m_FiredCount >= m_MaxProjectileCount)
      {
          return;
      }
      Projectile targetProjectile = m_ProjectilePool.m_Projectiles[m_FiredCount];
      targetProjectile.Launch(this);
      UpdateFiredCount(m_FiredCount + 1);
  }*/

    /*private IEnumerator Reload()
    {
        if(m_FiredCount == 0)
            yield break;
        m_AmmoOutput.text = "-";
        m_IsReloading = true;
        m_ProjectilePool.SetAllProjectiles(true);

        yield return new WaitForSeconds(m_ReloadTime);
        UpdateFiredCount(0);
        m_IsReloading = false;
    }*/

    /*private void UpdateFiredCount(int newValue)
    {
        m_FiredCount = newValue;
        m_AmmoOutput.text = (m_MaxProjectileCount - m_FiredCount).ToString();
    }*/

   

