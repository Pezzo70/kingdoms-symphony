using System;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;

public class DisplayScrollData : MonoBehaviour
{
    public LocalizedTextMeshProUGUI title;
    public LocalizedTextMeshProUGUI objective;
    public LocalizedTextMeshProUGUI effect;
    public GameObject manaContainer;
    public GameObject playerManaPrefab;
    private List<GameObject> _instantiadedMana = new List<GameObject>();

    void OnDisable()
    {
        for (int i = 0; i < _instantiadedMana.Count; i++)
        {
            Destroy(_instantiadedMana[i]);
        }

        _instantiadedMana.Clear();
        title.LocalizationKey = "";
        objective.LocalizationKey = "";
        effect.LocalizationKey = "";
    }

    public void SetDisplayData(Scroll scroll)
    {
        title.LocalizationKey = "Scrolls.Name." + (int)scroll.scrollID;
        objective.LocalizationKey = "Scrolls.Objective." + (int)scroll.scrollID;
        effect.LocalizationKey = "Scrolls.Effect." + (int)scroll.scrollID;

        effect.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", scroll.xFactor.ToString()),
                new Tuple<string, string>("-Y", scroll.yFactor.ToString()),
                new Tuple<string, string>("-Z", scroll.zFactor.ToString()),
                new Tuple<string, string>("-W", scroll.wFactor.ToString()),
            }
        );

        for (int i = 0; i < scroll.manaRequired; i++)
        {
            GameObject manaInstance = Instantiate(playerManaPrefab, manaContainer.transform);
            _instantiadedMana.Add(manaInstance);
        }
    }
}
