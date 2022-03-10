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

    public float Force = 10;

    private bool isAttached = false;
    private bool softAttach = false; //soft attach being true means the player has to hold the trigger to keep item in hand
    
    private bool particlesActive = false;

    // private bool m_IsReloading = false;
    private Hand AttachedHand = null;
    private Interaction interactionSystem = null;
    private Pickup pickup = null;

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
    }

    private void Update()
    {
        
        if (!isAttached)
        {
            return;
        }

        if (AttachedHand is null)
        {
            AttachedHand = gameObject.transform.parent.gameObject.GetComponent(typeof(Hand)) as Hand;
        }
        //print(SteamVR_Actions._default.Squeeze.GetAxis(AttachedHand.handType));
        if ( grabAction.GetStateDown(AttachedHand.handType)|| Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(!particlesActive)
            {
                particles.gameObject.SetActive(true);
                particlesActive = true;
                StopAllCoroutines();
                StartCoroutine(Fire());
            }
            else
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
                projectile.gameObject.SetActive(false);
            }
        }

        if (releaseAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(AttachedHand, gameObject, ref isAttached);
            AttachedHand = null;
            isAttached = false;
        }

    }

    private void LateUpdate()
    {
        isAttached = interactionSystem.checkAttached(gameObject);
    }

    private IEnumerator Fire()
    {
        while(particlesActive)
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);
            projectile.transform.position = barrel.transform.position;
            projectile.transform.rotation = barrel.transform.rotation;
            projectile.gameObject.SetActive(true);
            projectileRigidBody.AddRelativeForce(Vector3.forward * Force, ForceMode.Impulse);
            yield return wait;
            SetInnactive();
        }
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

   

