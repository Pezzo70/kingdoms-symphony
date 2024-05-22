using System;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Audio;
using Kingdom.Level;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public GameObject onPhaseVictoryUI;

    public GameObject onVictoryUI;
    public LocalizedTextMeshProUGUI onVictoryEnemiesBanished;
    public LocalizedTextMeshProUGUI onVictoryEnemiesXP;
    public LocalizedTextMeshProUGUI onVictoryScrollsUsed;
    public LocalizedTextMeshProUGUI onVictoryScrollsAccomplished;
    public LocalizedTextMeshProUGUI onVictoryScrollsXP;
    public GameObject onVictoryLevelUp;

    public GameObject onDefeatUI;
    public LocalizedTextMeshProUGUI onDefeatEnemiesBanished;
    public LocalizedTextMeshProUGUI onDefeatEnemiesXP;
    public LocalizedTextMeshProUGUI onDefeatScrollsUsed;
    public LocalizedTextMeshProUGUI onDefeatScrollsAccomplished;
    public LocalizedTextMeshProUGUI onDefeatScrollsXP;
    public GameObject onDefeatLevelUp;

    void OnEnable()
    {
        EventManager.PhaseVictory += HandlePhaseVictory;
        EventManager.EndGameVictory += HandleEndGameVictory;
        EventManager.EndGameDefeat += HandleEndGameDefeat;
    }

    void OnDisable()
    {
        EventManager.PhaseVictory -= HandlePhaseVictory;
        EventManager.EndGameVictory -= HandleEndGameVictory;
        EventManager.EndGameDefeat -= HandleEndGameDefeat;
    }

    private void HandleEndGameDefeat()
    {
        var endGameInfo = PlaythroughContainer.Instance.GetEndGameInfoAndUpdatePlayerData(false);
        AudioSystem.Instance.Play(Kingdom.Enums.EndState.Defeat);
        onDefeatUI.SetActive(true);

        onDefeatEnemiesBanished.LocalizationKey = "InGame.Details.EnemiesDefeated";
        onDefeatEnemiesBanished.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.enemiesBanished.ToString())
            }
        );
        onDefeatEnemiesXP.LocalizationKey = "InGame.Details.EnemyXP";
        onDefeatEnemiesXP.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.enemiesXP.ToString())
            }
        );
        onDefeatScrollsUsed.LocalizationKey = "InGame.Details.ScrollsUsed";
        onDefeatScrollsUsed.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsUsed.ToString())
            }
        );
        onDefeatScrollsAccomplished.LocalizationKey = "InGame.Details.ScrollsAccomplished";
        onDefeatScrollsAccomplished.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsAccomplished.ToString())
            }
        );
        onDefeatScrollsXP.LocalizationKey = "InGame.Details.ScrollXP";
        onDefeatScrollsXP.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsXP.ToString())
            }
        );
        onDefeatLevelUp.SetActive(endGameInfo.leveledUp);
    }

    private void HandleEndGameVictory()
    {
        var endGameInfo = PlaythroughContainer.Instance.GetEndGameInfoAndUpdatePlayerData(true);
        AudioSystem.Instance.Play(Kingdom.Enums.EndState.RunVictory);
        onVictoryUI.SetActive(true);

        onVictoryEnemiesBanished.LocalizationKey = "InGame.Details.EnemiesDefeated";
        onVictoryEnemiesBanished.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.enemiesBanished.ToString())
            }
        );
        onVictoryEnemiesXP.LocalizationKey = "InGame.Details.EnemyXP";
        onVictoryEnemiesXP.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.enemiesXP.ToString())
            }
        );
        onVictoryScrollsUsed.LocalizationKey = "InGame.Details.ScrollsUsed";
        onVictoryScrollsUsed.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsUsed.ToString())
            }
        );
        onVictoryScrollsAccomplished.LocalizationKey = "InGame.Details.ScrollsAccomplished";
        onVictoryScrollsAccomplished.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsAccomplished.ToString())
            }
        );
        onVictoryScrollsXP.LocalizationKey = "InGame.Details.ScrollXP";
        onVictoryScrollsXP.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", endGameInfo.scrollsXP.ToString())
            }
        );
        onVictoryLevelUp.SetActive(endGameInfo.leveledUp);
    }

    private void HandlePhaseVictory()
    {
        AudioSystem.Instance.Play(Kingdom.Enums.EndState.LevelVictory);
        onPhaseVictoryUI.SetActive(true);
    }
}
