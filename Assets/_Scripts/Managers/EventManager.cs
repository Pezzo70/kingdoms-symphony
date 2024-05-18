using Kingdom.Enums;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction<LevelTransitionOption> LevelTransition;
    public static UnityAction SavePlayerData;
    public static UnityAction SavePlayerConfig;
}
