using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Enemies;
using Kingdom.Enums.Enemies;
using Kingdom.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyData : MonoBehaviour
{
    public LocalizedTextMeshProUGUI enemyName;
    public LocalizedTextMeshProUGUI description;
    public TextMeshProUGUI combatData;
    public TextMeshProUGUI moral;
    public GameObject manaContainer;
    public GameObject enemyManaPrefab;
    public TextMeshProUGUI infoIndex;
    public Image enemyPortrait;
    public Sprite[] enemySprites;
    private List<GameObject> _instantiadedMana = new List<GameObject>();
    private List<string> _allInfo = new List<string>();
    private int _infoIndex;

    void OnDisable()
    {
        for (int i = 0; i < _instantiadedMana.Count; i++)
        {
            Destroy(_instantiadedMana[i]);
        }

        _allInfo.Clear();
        _instantiadedMana.Clear();
        enemyName.LocalizationKey = "";
        description.LocalizationKey = "";
        combatData.text = "";
        moral.text = "";
    }

    public void SetDisplayData(Enemy enemy)
    {
        enemyName.LocalizationKey = "Enemies.Entity.Name." + (int)enemy.enemyID;
        description.LocalizationKey = "Enemies.Entity.Description." + (int)enemy.enemyID;

        var playerData = PlayerContainer.Instance.PlayerData;
        var attacksUnlocked = playerData.EnemyAttacksUnlocked;
        var advantagesUnlocked = playerData.EnemyAdvantagesUnlocked;
        var disadvantagesUnlocked = playerData.EnemyDisadvangatesUnlocked;

        string locked = LocalizationManager.Localize("Menu.Locked");

        foreach (var attack in enemy.attacks)
            AddOnAllInfo(
                CombatInfo.Attack,
                attacksUnlocked,
                enemy.enemyID,
                attack.enemyAttackID,
                "Enemies.Attack.Name.",
                "Enemies.Attack.Description.",
                locked,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>("-X", attack.xFactor.ToString()),
                    new Tuple<string, string>("-Y", attack.yFactor.ToString()),
                    new Tuple<string, string>("-Z", attack.zFactor.ToString()),
                    new Tuple<string, string>("-W", attack.wFactor.ToString()),
                    new Tuple<string, string>("-J", attack.jFactor.ToString()),
                    new Tuple<string, string>("-M", attack.mFactor.ToString()),
                    new Tuple<string, string>("-L", attack.lFactor.ToString()),
                }
            );

        foreach (var advantage in enemy.advantages)
            AddOnAllInfo(
                CombatInfo.Advantage,
                advantagesUnlocked,
                enemy.enemyID,
                advantage.enemyAdvantageID,
                "Enemies.Advantages.Name.",
                "Enemies.Advantages.Description.",
                locked,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>("-X", advantage.xFactor.ToString()),
                    new Tuple<string, string>("-Y", advantage.yFactor.ToString()),
                    new Tuple<string, string>("-Z", advantage.zFactor.ToString()),
                }
            );

        foreach (var disadvantage in enemy.disadvantages)
            AddOnAllInfo(
                CombatInfo.Disadvantage,
                disadvantagesUnlocked,
                enemy.enemyID,
                disadvantage.enemyDisadvantageID,
                "Enemies.Disadvantages.Name.",
                "Enemies.Disadvantages.Description.",
                locked,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>("-X", disadvantage.xFactor.ToString()),
                    new Tuple<string, string>("-Y", disadvantage.yFactor.ToString()),
                    new Tuple<string, string>("-Z", disadvantage.zFactor.ToString()),
                }
            );

        moral.text = enemy.moral.ToString();

        for (int i = 0; i < enemy.manaPerTurn; i++)
        {
            GameObject manaInstance = Instantiate(enemyManaPrefab, manaContainer.transform);
            _instantiadedMana.Add(manaInstance);
        }

        combatData.text = _allInfo[_infoIndex];
        infoIndex.text = (_infoIndex + 1).ToString();

        enemyPortrait.sprite = enemySprites[(int)enemy.enemyID];
    }

    public void NextInfo()
    {
        _infoIndex = _infoIndex == _allInfo.Count - 1 ? 0 : _infoIndex + 1;
        combatData.text = _allInfo[_infoIndex];
        infoIndex.text = (_infoIndex + 1).ToString();
    }

    public void PreviousInfo()
    {
        _infoIndex = _infoIndex == 0 ? _allInfo.Count - 1 : _infoIndex - 1;
        combatData.text = _allInfo[_infoIndex];
        infoIndex.text = (_infoIndex + 1).ToString();
    }

    private void AddOnAllInfo<T, TT>(
        CombatInfo ci,
        ReadOnlyDictionary<T, List<TT>> targetCollection,
        T enemyID,
        TT infoID,
        string nameKey,
        string descriptionKey,
        string locked,
        Tuple<string, string>[] replacements
    )
    {
        if (targetCollection[enemyID].Contains(infoID))
        {
            int convertedInfoID = Convert.ToInt32(infoID);
            string combatInfo = LocalizationManager.Localize("Enemies.Combat.Info." + (int)ci);
            string name = LocalizationManager.Localize(nameKey + convertedInfoID);
            string description = LocalizationManager.Localize(descriptionKey + convertedInfoID);

            string completeInfo = $"{combatInfo} {name}: \n {description}";

            if (replacements.Length > 0)
                foreach (var replacement in replacements)
                    completeInfo = completeInfo.Replace(replacement.Item1, replacement.Item2);

            _allInfo.Add(completeInfo);
        }
        else
        {
            _allInfo.Add(locked);
        }
    }
}
