using System;
using Kingdom.Enums;
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
}
