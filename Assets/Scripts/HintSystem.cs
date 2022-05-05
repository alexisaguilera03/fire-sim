
using System.Collections;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class HintSystem : MonoBehaviour
{
    public enum Hint //todo: change order
    {
        None = -1,
        Duck,
        GrabObjects,
        DropObjects,
        Extinguisher,
        Hose,
        DeathDoor
    }
    public Hint hint;
    public Sprite[] Sprites;

    private Image currentHint;

    private Sprite nextHintSprite;
    public Hint activeHint = Hint.None;
    private Hint previousHint = Hint.None;

    private bool flash = false;
    private bool flashing = false;

    public bool hintTaken = false;

    public bool test = false; //delete when done testing

    public float offset; 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, true);
        currentHint = gameObject.GetComponent<Image>();
        currentHint.color = new Color(currentHint.color.r, currentHint.color.g, currentHint.color.b, 0);
        if (currentHint.sprite == null)
        {
            //getNextHint(FirstHint); //remove when done testing
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(test) getNextHint(Hint.GrabObjects);
        if (flash && hintTaken)
        {
            flash = false;
            hintTaken = false;
            removeHint();
        }
        if (flash && !flashing) StartCoroutine(flashHint());
    }


    void getNextHint(Hint hint)
    {
        if (test) test = false;
        if (activeHint == Hint.None )
        {
            activeHint = hint;
        }
        else
        {
            previousHint = activeHint;
            activeHint = hint;
            if (previousHint == activeHint && !hintTaken){
                return; //prevent hint that is currently being displayed from displaying again
            }
            else
            {
                removeHint();
            }
        }

        this.hint = hint;
        
        switch (hint)
        {
            case Hint.Duck:
                currentHint.sprite = Sprites[0];
                break;
            case Hint.GrabObjects:
                currentHint.sprite = Sprites[1];
                break;
            case Hint.None:
                currentHint.sprite = Sprites[2];
                break;
            case Hint.DropObjects:
                currentHint.sprite = Sprites[3];
                break;
            case Hint.Extinguisher:
                currentHint.sprite = Sprites[4];
                break;
            case Hint.Hose:
                currentHint.sprite = Sprites[5];
                break;
            case Hint.DeathDoor:
                currentHint.sprite = Sprites[6];
                break;
            default:
                return;
        }

        StartCoroutine(fadeHint());
    }


    public void displayHint(Hint hint)
    {
        getNextHint(hint);
        
    }

    public void displayHint(Hint hint, int delay)
    {
        StartCoroutine(waitUntilDisplayHint(delay, hint));
    }

    public void displayHint(Hint hint, int startDelay, int removeDelay)
    {
        if (startDelay == 0 && removeDelay == 0)
        {
            displayHint(hint);
            return;
        }
        if (startDelay == 0)
        {
            displayHint(hint);
            Invoke("removeHint", removeDelay);
        }
        else
        {
            StartCoroutine(waitUntilDisplayHint(startDelay, hint));
            Invoke("removeHint", removeDelay + startDelay);
        }
    }

    public void removeHint()
    {
        hint = Hint.None;
        flash = false;
        StopAllCoroutines();
        CancelInvoke();
        currentHint.color = new Color(currentHint.color.r, currentHint.color.g, currentHint.color.b, 0);

    }

    
    IEnumerator waitUntilDisplayHint(int seconds, Hint hint)
    {
        yield return new WaitForSeconds(seconds);
        displayHint(hint);
        
    }

    IEnumerator fadeHint()
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            currentHint.color = new Color(currentHint.color.r, currentHint.color.g, currentHint.color.b, i);
            yield return null;
        }

        flash = true;
        StartCoroutine(flashHint());
    }

    IEnumerator flashHint()
    {
        const float opacityMin = 0.2f;
        flashing = true;

        for (float i = 1; i > opacityMin; i -= Time.deltaTime * offset)
        {
            currentHint.color = new Color(currentHint.color.r, currentHint.color.g, currentHint.color.b, i);
            yield return null;
        }

        for (float i = opacityMin; i < 1; i += Time.deltaTime * offset) 
        {
            currentHint.color = new Color(currentHint.color.r, currentHint.color.g, currentHint.color.b, i);
            yield return null;
        }

        flashing = false;
    }


}
