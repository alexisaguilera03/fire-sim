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

    private bool isAttached = false;
    private bool softAttach = false; //soft attach being true means the player has to hold the trigger to keep item in hand
    
    private bool particlesActive = false;

    private bool m_IsReloading = false;
    private Hand AttachedHand = null;
    private Interaction interactionSystem = null;



    private void Start()
    {
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

        //print(SteamVR_Actions._default.Squeeze.GetAxis(AttachedHand.handType));
        if ( grabAction.GetStateDown(AttachedHand.handType)|| Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(!particlesActive)
            {
                particles.gameObject.SetActive(true);
                particlesActive = true;
            }
            else
            {
                particles.gameObject.SetActive(false);
                particlesActive = false;
            }
        }

        if (releaseAction.GetStateDown(AttachedHand.handType) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            interactionSystem.Release(AttachedHand, gameObject, ref isAttached);
            AttachedHand = null;
            isAttached = false;
        }

    }
    void HandHoverUpdate(Hand hand)
    {
       interactionSystem.OnHandHover(hand, gameObject, ref isAttached);
       if (!isAttached) return;
       AttachedHand = hand;
       softAttach = false;
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

   

