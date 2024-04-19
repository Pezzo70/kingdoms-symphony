using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class LanguageControl : MonoBehaviour
{
    ToggleGroup LanguageGroup;
    // Start is called before the first frame update
    void Start()
    {
        LanguageGroup = GetComponent<ToggleGroup>();
        LocalizationManager.Read();


        var togglesInGroup = GetComponentsInChildren<Toggle>();
        for(int i = 0; i < togglesInGroup.Length; i++)
        {
            int toggleID = i;
            var toggle = togglesInGroup[i];
            toggle.onValueChanged.AddListener( delegate{ OnLanguageSelected(toggle, toggleID); });

            if(toggle.isOn) OnLanguageSelected(toggle, toggleID);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnLanguageSelected(Toggle toggle, int toggleID)
    {
        if(toggle.isOn)
            switch (toggleID)
            {
                case 0:
                    LocalizationManager.Language = "Portuguese";
                    break;
                case 1:
                    LocalizationManager.Language = "English";
                    break;
            }
        else
            Debug.Log("Deselected");

        Debug.Log(LocalizationManager.Language + "" + toggleID);
    }
    
}
