using Assets.SimpleLocalization.Scripts;
using Kingdom.Enums.Tips;
using UnityEngine;

public class DisplayTipData : MonoBehaviour
{
    public LocalizedTextMeshProUGUI tipName;
    public LocalizedTextMeshProUGUI description;

    void OnDisable()
    {
        tipName.LocalizationKey = "";
        description.LocalizationKey = "";
    }

    public void SetDisplayData(Tip tip)
    {
        tipName.LocalizationKey = "Tips.Name." + (int)tip.tipID;
        description.LocalizationKey = "Tips.Description." + (int)tip.tipID;
    }
}
