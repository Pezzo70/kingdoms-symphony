using Kingdom.Enums.MusicTheory;
using UnityEngine;

namespace Kingdom.Enemies
{
    [CreateAssetMenu(
        fileName = "EnemyAdvantage",
        menuName = "ScriptableObjects/Enemies/EnemyAdvantage",
        order = 3
    )]
    public class EnemyAdvantage : ScriptableObject
    {
        public EnemyAdvantageID enemyAdvantageID;
        public int targetMeasures;
        public Clef[] validClef;
        public Chords[] validChords;
        public Scale[] validScales;
        public Modes[] validModes;

        [Header(
            "Factors (Applied on Scroll effect) - Must check GDD\nPercentage values does not use the 0-1 floating point interval."
        )]
        public float xFactor;
        public float yFactor;
        public float zFactor;
    }
}
