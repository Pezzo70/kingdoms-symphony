using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Enums.Tips;
using Kingdom.Player;
using UnityEngine;
using UnityEngine.UI;

public class TipOrScrollUI : MonoBehaviour
{
    public ScriptableObject displayTarget;
    private LocalizedTextMeshProUGUI _childTextMeshProUGUI;
    private Selectable _selectable;

    void Awake()
    {
        _childTextMeshProUGUI = GetComponentInChildren<LocalizedTextMeshProUGUI>();
        _selectable = GetComponent<Selectable>();
    }

    void Start()
    {
        switch (displayTarget)
        {
            case Tip tip:
                if (tip.isEnemy)
                {
                    if (PlayerContainer.Instance.playerData.EnemyInfoUnlocked[tip.enemyID])
                    {
                        _childTextMeshProUGUI.LocalizationKey =
                            "Enemies.Entity.Name." + (int)tip.enemyID;
                    }
                    else
                    {
                        _childTextMeshProUGUI.LocalizationKey = "Menu.Locked";
                        _selectable.interactable = false;
                    }

                    gameObject.SetActive(false);
                }
                else
                {
                    _childTextMeshProUGUI.LocalizationKey =
                        _childTextMeshProUGUI.LocalizationKey + (int)tip.tipID;
                }
                break;
            case Scroll scroll:
                if (
                    PlayerContainer
                        .Instance
                        .playerData
                        .LevelPerCharacter
                        .Where(obj => scroll.unlockedFor.Contains(obj.Key))
                        .Any(character => character.Value.level >= scroll.levelRequired)
                )
                {
                    _childTextMeshProUGUI.LocalizationKey =
                        _childTextMeshProUGUI.LocalizationKey + (int)scroll.scrollID;
                }
                else
                {
                    _childTextMeshProUGUI.LocalizationKey = "Menu.Locked";
                    _selectable.interactable = false;
                }

                break;
        }
    }
}
