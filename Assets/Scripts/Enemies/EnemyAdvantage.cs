using MusicTheory;
using UnityEngine;

namespace Enemies
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
        public Chords[] validChords;
        public Scale[] validScales;
        public Modes[] validModes;
        public float xFactor;
        public float yFactor;
        public float zFactor;
    }
}
