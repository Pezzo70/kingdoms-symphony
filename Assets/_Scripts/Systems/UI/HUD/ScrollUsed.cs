using System;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Effects;
using UnityEngine;

public class ScrollUsed : MonoBehaviour
{
    public ScrollDTO scrollDTO;
    public LocalizedTextMeshProUGUI objective;
    public LocalizedTextMeshProUGUI target;

    public void SetData(ScrollDTO scrollDTO)
    {
        this.scrollDTO = scrollDTO;
        objective.LocalizationKey = "Scrolls.Objective." + (int)scrollDTO.Scroll.scrollID;
        if (scrollDTO.RandomTargetsNames.Length > 0)
        {
            target.LocalizationKey = "Scrolls.Menu.Targets";
            string concat = string.Join(" ,", scrollDTO.RandomTargetsNames);
            target.Replace(new Tuple<string, string>[] { new Tuple<string, string>("-X", concat) });
        }
        else
        {
            target.gameObject.SetActive(false);
        }
    }
}
