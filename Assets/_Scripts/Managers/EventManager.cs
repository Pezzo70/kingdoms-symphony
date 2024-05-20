using System;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Level;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction<LevelTransitionOption> LevelTransition;
    public static UnityAction SavePlayerData;
    public static UnityAction SavePlayerConfig;

    public static UnityAction<(
        string localizedKey,
        bool isError,
        bool needConfirmation,
        Action confirmationCallback,
        bool timed,
        float seconds,
        bool closable,
        Vector3 targetPosition,
        bool shouldDisplayOpacityPanel
    )> ShowPopUp;

    public static UnityAction<Enemy> EnemyBanished;
    public static UnityAction<Enemy> EnemyEncountered;
    public static UnityAction<EnemyAttack> EnemyAttackExecuted;
    public static UnityAction<EnemyAdvantage> EnemyAdvantageTriggered;
    public static UnityAction<EnemyDisadvantage> EnemyDisadvantageTriggered;
    public static UnityAction<Scroll> ScrollUsed;
    public static UnityAction<Scroll> ScrollAccomplished;

    public static UnityAction<Level> LoadNewLevel;
    public static UnityAction OnPlayerMoralChange;
    public static UnityAction OnPlayerManaChange;
    public static UnityAction EndGameVictory;
    public static UnityAction EndGameDefeat;
    public static UnityAction PauseGame;
    public static UnityAction UnpauseGame;

    public static UnityAction<float> EnemiesDamaged;
    public static UnityAction EnemiesRegainMana;
}
