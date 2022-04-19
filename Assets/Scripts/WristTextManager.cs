using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WristTextManager : MonoBehaviour
{

    public static WristTextManager Instance;
    public TextMeshProUGUI objectiveText;
    private void Start()
    {
        Instance = this;
    }

    public void SetObjectiveText(string s)
    {
        objectiveText.SetText(s);
    }

}
