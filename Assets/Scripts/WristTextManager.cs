using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WristTextManager : MonoBehaviour
{

    public static WristTextManager Instance;
    private TextMeshProUGUI objectiveText;

    private void Start()
    {
        objectiveText = GetComponent<TextMeshProUGUI>();
        Instance = this;
    }

    public void SetObjectiveText(string s)
    {
        objectiveText.SetText(s);
    }

}
