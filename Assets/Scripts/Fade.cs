using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fader;
    private Color currentColor;
    private Outline[] outlines;
    public bool fadein = false;

    public bool fadeout = false;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        fader = gameObject.GetComponent<Image>();
        fader.color = Color.white;
        currentColor = fader.color;
        if (GameObject.FindGameObjectWithTag("InteractionSystem") != null)
        {
            outlines = GameObject.FindGameObjectWithTag("InteractionSystem").GetComponentsInChildren<Outline>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadein)
        {
            
            StartCoroutine(fadeIn(Color.black, 1));
        }

        if (fadeout)
        {
            StartCoroutine(fadeOut(1));
        }
    }

    public void FadeIn(Color targetColor, float seconds)
    {
        StartCoroutine(fadeIn(targetColor, seconds));
    }

    public void FadeIn(Color targetColor, float seconds, int delay)
    {
        StartCoroutine(waitUntilFade(delay, targetColor, seconds));
    }

    public void FadeOut(float seconds)
    {
        StartCoroutine(fadeOut(seconds));
    }

    void setOutlinesOff()
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    void resetOutlines()
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = true;
        }
    }

    IEnumerator fadeIn(Color targetColor, float seconds)
    {
        if (outlines != null)
        {
            setOutlinesOff();
        }
        fadein = false;
        fader.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
        for (float i = 0; i < seconds; i += Time.deltaTime/seconds)  //this line was changed! todo: check if division works
        {
            fader.color = new Color(targetColor.r, targetColor.g, targetColor.b, i);
            yield return null;
        }

        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 1);
        currentColor = fader.color;
    }

    IEnumerator fadeOut(float seconds)
    {
        yield return new WaitForSeconds(0.5f);
        fadeout = false;
        for (float i = seconds; i > 0; i -= Time.deltaTime)
        {
            fader.color = new Color(currentColor.r, currentColor.g, currentColor.b, i);
            yield return null;
        }

        if (outlines != null)
        {
            resetOutlines();
        }
        fader.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
    }

    IEnumerator waitUntilFade(int delay, Color targetColor, float seconds)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(fadeIn(targetColor, seconds));
    }
}
