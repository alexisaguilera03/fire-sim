using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SmokeDetection : MonoBehaviour
{
    private GameObject Teleporting;

    private Fade fader;

    private bool inSmoke = false;
    private bool hintShown = false;

    private GameObject head;

    private HintSystem hintSystem;

    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.FindGameObjectWithTag("UI").GetComponent<Fade>();
        head = GameManager.player.GetComponent<Player>().headCollider.gameObject;
        hintSystem = GameObject.FindGameObjectWithTag("HintSystem").GetComponent<HintSystem>();
        Teleporting = GameObject.FindGameObjectWithTag("Teleporting");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == head)
        {
            inSmoke = true;
            StartCoroutine(showHint());
            fader.FadeIn(Color.grey, 5f);
            Teleporting.SetActive(false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == head)
        {
            if (hintShown)
            {
                hintSystem.hintTaken = true;
            }
            else
            {
                StopAllCoroutines();
            }
            inSmoke = false;
            fader.StopAllCoroutines();
            fader.FadeOut(1);
            Teleporting.SetActive(true);
        }
    }

    IEnumerator showHint()
    {
        yield return new WaitForSeconds(3);
        hintSystem.displayHint(HintSystem.Hint.Duck);
        hintShown = true;

    }
}
