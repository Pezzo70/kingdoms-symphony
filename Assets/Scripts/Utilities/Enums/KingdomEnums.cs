namespace KingdomEnums
{
    public enum KingdomStage
    {
        Menu,
        Florest,
        Dungeon,
        Cave,
        Underground
    }

    public enum ActorAudioTypes
    {
        Damage,
        Attack
    }

    public enum EndState
    {
        LevelVictory = 0,
        Defeat = 1,
        RunVictory = 2
    }

    public enum UIAction
    {
        Hover,
        Submit,
        Cancel,
        PopUp,
        Pause,
        Return
    }
    public enum NotationBehaviour
    {
        Pause, Note
    }

    public enum NotationOrientation
    {
        Up, Down, Center
    }

    public enum Tempo
    {
        Whole = 1,
        Half = 2,
        Quarter = 4,
        Eighth = 8,
        Sixteenth = 16
    }
}
