using System;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Enemies;
using Kingdom.Enums.Tips;
using Kingdom.Player;
using UnityEngine;
using UnityEngine.UI;

public class TipOrScrollUI : MonoBehaviour
{
    public ScriptableObject displayTarget;
    private GameObject _targetContainer;
    private LocalizedTextMeshProUGUI _childTextMeshProUGUI;
    private Selectable _selectable;
    private Action _handleClick = () => { };

    void Awake()
    {
        _childTextMeshProUGUI = GetComponentInChildren<LocalizedTextMeshProUGUI>();
        _selectable = GetComponent<Selectable>();
    }

    void Start()
    {
        GameObject tipOrScrollMenu = this.transform.parent.parent.parent.gameObject;

        _handleClick += () =>
        {
            ScrollRect tipOrScrollMenuScrollRect = tipOrScrollMenu.GetComponent<ScrollRect>();
            for (int i = 1; i < tipOrScrollMenu.transform.childCount; i++)
            {
                tipOrScrollMenu.transform.GetChild(i).gameObject.SetActive(false);
            }
            tipOrScrollMenuScrollRect.enabled = false;
        };

        switch (displayTarget)
        {
            case Tip tip:
                if (tip.isEnemy)
                {
                    if (PlayerContainer.Instance.playerData.EnemyInfoUnlocked[tip.enemyID])
                    {
                        _childTextMeshProUGUI.LocalizationKey =
                            "Enemies.Entity.Name." + (int)tip.enemyID;

                        _targetContainer = tipOrScrollMenu.transform.Find("EnemyData").gameObject;

                        _handleClick += () =>
                        {
                            _targetContainer.SetActive(true);
                            _targetContainer
                                .GetComponent<DisplayEnemyData>()
                                .SetDisplayData(
                                    EnemiesContainer
                                        .Instance
                                        .enemies
                                        .First(obj => obj.enemyID == tip.enemyID)
                                );
                        };
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

                    _targetContainer = tipOrScrollMenu.transform.Find("TipData").gameObject;

                    _handleClick += () =>
                    {
                        _targetContainer.SetActive(true);
                        _targetContainer.GetComponent<DisplayTipData>().SetDisplayData(tip);
                    };
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

                    _targetContainer = tipOrScrollMenu.transform.Find("ScrollData").gameObject;

                    _handleClick += () =>
                    {
                        _targetContainer.SetActive(true);
                        _targetContainer.GetComponent<DisplayScrollData>().SetDisplayData(scroll);
                    };
                }
                else
                {
                    _childTextMeshProUGUI.LocalizationKey = "Menu.Locked";
                    _selectable.interactable = false;
                }

                break;
        }
    }

    public void HandleClick() => _handleClick.Invoke();
}
