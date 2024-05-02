using MusicTheory;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(
        fileName = "EnemyDisadvantage",
        menuName = "ScriptableObjects/Enemies/EnemyDisadvantage",
        order = 4
    )]
    public class EnemyDisadvantage : ScriptableObject
    {
        public EnemyDisadvantageID enemyDisadvantageID;
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
