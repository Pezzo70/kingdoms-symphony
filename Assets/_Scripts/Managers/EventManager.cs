using System;
using Kingdom.Effects;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.Scrolls;
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

    public static UnityAction<EnemyID> EnemyBanished;
    public static UnityAction<EnemyID> EnemyEncountered;
    public static UnityAction<EnemyID, EnemyAttackID> EnemyAttackExecuted;
    public static UnityAction<EnemyID, EnemyAdvantageID> EnemyAdvantageTriggered;
    public static UnityAction<EnemyID, EnemyDisadvantageID> EnemyDisadvantageTriggered;
    public static UnityAction<ScrollID> ScrollUsed;
    public static UnityAction<ScrollID> ScrollAccomplished;
    public static UnityAction<ScrollID> ScrollFailed;

    public static UnityAction OnPlayerMoralChange;
    public static UnityAction OnPlayerManaChange;
    public static UnityAction OnPlayersDeath;

    public static UnityAction PhaseVictory;
    public static UnityAction EndGameVictory;
    public static UnityAction EndGameDefeat;
    public static UnityAction<bool> OnVictory;

    public static UnityAction PauseGame;
    public static UnityAction UnpauseGame;
    public static UnityAction<bool> CantPause;

    public static UnityAction<float, float> EnemiesDamaged;
    public static UnityAction EnemiesRegainMana;
    public static UnityAction EnemiesEndTakingDamage;

    public static UnityAction<Turn> TurnChanged;
    public static UnityAction NextEnemy;
    public static UnityAction NextEnemyTakesDamage;

    public static UnityAction<Scroll> CastScroll;

    public static UnityAction<Scroll> OpenScroll;
    public static UnityAction<ScrollDTO> AddScroll;
    public static UnityAction<ScrollDTO> ScrollRemoved;
    public static UnityAction<Scroll> BurnScroll;
    public static UnityAction<EffectDTO> AddEffect;
    public static UnityAction<EffectDTO> RemoveEffect;

    public static UnityAction<EffectDTO> DamageEffectExecuted;

    public static UnityAction<bool> MusicSheetOpen;
}
