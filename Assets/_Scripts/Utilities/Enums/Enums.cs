namespace Kingdom.Enums
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

    public enum Chord
    {

    }
}

namespace Kingdom.Enums.FX
{
    public enum FXID
    {
        Portal = 0,
        Pft = 1
    }

    public enum FXState
    {
        Start = 0,
        Idle = 1,
        End = 1
    }
}

namespace Kingdom.Enums.Player
{
    public enum CharacterID
    {
        Roddie = 0,
    }
}

namespace Kingdom.Enums.Scrolls
{
    public enum ScrollID
    {
        FirstMajorNotes = 0,
        ExploringMajorMelodies = 1,
        PausesToRest = 2,
        ChangingStrategies = 3,
        BetweenTones = 4,
        BetweenSemitones = 5,
        MoreChords = 6,
        KeynoteStrength = 7,
        Ambiguity = 8,
        PassageAndControl = 9,
        CommonProgress = 10,
        HealingThroughArpeggios = 11,
        BetweenScales = 12,
        Melodic = 13,
        PianistWithModes = 14
    }
}

namespace Kingdom.Enums.MusicTheory
{
    public enum Chords
    {
        CMajor = 0,
        CSharpMajor = 1,
        DMajor = 2,
        EbMajor = 3,
        EMajor = 4,
        FMajor = 5,
        FSharpMajor = 6,
        GMajor = 7,
        AbMajor = 8,
        AMajor = 9,
        BbMajor = 10,
        BMajor = 11,
        CMinor = 12,
        CSharpMinor = 13,
        DMinor = 14,
        EbMinor = 15,
        EMinor = 16,
        FMinor = 17,
        FSharpMinor = 18,
        GMinor = 19,
        AbMinor = 20,
        AMinor = 21,
        BbMinor = 22,
        BMinor = 23
    }

    public enum Scale
    {
        MajorC = 0,
        MajorD = 1,
        MajorE = 2,
        MajorF = 3,
        MajorG = 4,
        MajorA = 5,
        MajorB = 6,
        MinorC = 7,
        MinorD = 8,
        MinorE = 9,
        MinorF = 10,
        MinorG = 11,
        MinorA = 12,
        MinorB = 13,
    }

    public enum Modes
    {
        Ionian = 0,
        Dorian = 1,
        Phrygian = 2,
        Lydian = 3,
        Mixolydian = 4,
        Aeolian = 5,
        Locrian = 6
    }

    public enum Clef
    {
        G = 0,
        F = 1
    }
}

namespace Kingdom.Enums.Enemies
{
    public enum EnemyID
    {
        OutOfTuneGoblin = 0,
        SteplessWerewolf = 1,
        UnshakenBones = 2,
        SoundlessShadows = 3,
        SilencedClaws = 4,
        SoundWatcher = 5,
        AlienCaptain = 6,
        AbyssalVisitor = 7
    }

    public enum EnemyAttackID
    {
        Shove = 0,
        RythmlessClaps = 1,
        RythmlessClaws = 2,
        WaywardSteps = 3,
        UncontrollableGloves = 4,
        AirheadHeadbutt = 5,
        SilenceTheMind = 6,
        PoisonedFingers = 7,
        CuttingStrings = 8,
        ScratchingAndControlling = 9,
        StickyTentacles = 10,
        JudgeEyes = 11,
        SonicCrush = 12,
        HarmonicNullification = 13,
        AbysmalForce = 14,
        TroubledMind = 15
    }

    public enum EnemyAdvantageID
    {
        GoblinsWill = 0,
        WerewolfsWill = 1,
        BonesWill = 2,
        MindsWill = 3,
        ClawsWill = 4,
        HealingEyes = 5,
        WatchersWill = 6,
        CaptainsWill = 7,
        PhngluiMglwNafhCthulhuRlyehWgahNaglFhtagn = 8
    }

    public enum EnemyDisadvantageID
    {
        HatefulMelodies = 0,
        CantPauseHowls = 1,
        ShakesTooMuch = 2,
        Headaches = 3,
        SmoothingTheClaws = 4,
        RightAtTheEyes = 5,
        HatefulModes = 6,
        ImNotMyFather = 7,
    }

    public enum AttackType
    {
        Impact = 0,
        Sharp = 1,
        Mind = 2,
        Cosmic = 3
    }
}
