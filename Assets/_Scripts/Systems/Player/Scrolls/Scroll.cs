using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Player;
using Kingdom.Enums.Scrolls;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "ScriptableObjects/Scrolls", order = 1)]
public class Scroll : ScriptableObject
{
    public ScrollID scrollID;

    [Range(1, 10)]
    public int levelRequired;
    public CharacterID[] unlockedFor;
    public int manaRequired;
    public int cooldown;
    public int targetMeasures;
    public Clef[] validClef;
    public Chords[] validChords;
    public Scale[] validScales;
    public Modes[] validModes;
    public int XP;

    [Header(
        "Factors (Applied on Scroll effect) - Must check GDD\nPercentage values does not use the 0-1 floating point interval."
    )]
    public float xFactor;
    public float yFactor;
    public float zFactor;
    public float wFactor;
}
