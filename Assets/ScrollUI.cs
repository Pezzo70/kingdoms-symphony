using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

public class ScrollUI : MonoBehaviour
{
    public Scroll scroll;
    void Start()
    {
        GetComponentInChildren<LocalizedTextMeshProUGUI>().LocalizationKey = GetComponentInChildren<LocalizedTextMeshProUGUI>().LocalizationKey + (int)scroll.scrollID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
