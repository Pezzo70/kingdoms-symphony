using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Enemies;
using TMPro;
using UnityEngine;

public class DisplayEnemyData : MonoBehaviour
{
    public LocalizedTextMeshProUGUI enemyName;
    public LocalizedTextMeshProUGUI description;
    public LocalizedTextMeshProUGUI combatData;
    public TextMeshProUGUI moral;
    public GameObject manaContainer;
    public GameObject enemyManaPrefab;
    private List<GameObject> _instantiadedMana = new List<GameObject>();
    private List<string> _allInfo = new List<string>();
    private int _infoIndex;

    void OnDisable()
    {
        for (int i = 0; i < _instantiadedMana.Count; i++)
        {
            Destroy(_instantiadedMana[i]);
        }

        _instantiadedMana.Clear();
        enemyName.LocalizationKey = "";
        description.LocalizationKey = "";
        combatData.LocalizationKey = "";
        moral.text = "";
    }

    public void SetDisplayData(Enemy enemy)
    {
        enemyName.LocalizationKey = "Enemies.Entity.Name." + (int)enemy.enemyID;
        description.LocalizationKey = "Enemies.Entity.Description." + (int)enemy.enemyID;

        foreach (var attack in enemy.attacks) { }

        foreach (var advantage in enemy.advantages) { }

        foreach (var disadvantage in enemy.disadvantages) { }
    }

    public void NextInfo() { }

    public void PreviousInfo() { }
}
