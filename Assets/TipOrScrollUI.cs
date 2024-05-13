using Assets.SimpleLocalization.Scripts;
using Kingdom.Enums.Tips;
using UnityEngine;

public class TipOrScrollUI : MonoBehaviour
{
    public ScriptableObject displayTarget;
    private LocalizedTextMeshProUGUI _childTextMeshProUGUI;

    void Awake()
    {
        _childTextMeshProUGUI = GetComponentInChildren<LocalizedTextMeshProUGUI>();
    }

    void Start()
    {
        switch (displayTarget)
        {
            case Tip tip:
                if (tip.isEnemy)
                {
                    _childTextMeshProUGUI.LocalizationKey =
                        "Enemies.Entity.Name." + (int)tip.enemyID;
                }
                else
                {
                    _childTextMeshProUGUI.LocalizationKey =
                        _childTextMeshProUGUI.LocalizationKey + (int)tip.tipID;
                }
                break;
            case Scroll scroll:
                _childTextMeshProUGUI.LocalizationKey =
                    _childTextMeshProUGUI.LocalizationKey + (int)scroll.scrollID;
                break;
        }
    }

    void Update() { }
}
