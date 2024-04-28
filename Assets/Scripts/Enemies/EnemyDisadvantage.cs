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
        public Chords[] validChords;
        public Scale[] validScales;
        public Modes[] validModes;
        public float xFactor;
        public float yFactor;
        public float zFactor;
    }
}
