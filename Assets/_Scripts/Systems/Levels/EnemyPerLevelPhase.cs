using Kingdom.Enemies;
using UnityEngine;

namespace Kingdom.Level
{
    [CreateAssetMenu(
        fileName = "EnemyPerLevelPhase",
        menuName = "ScriptableObjects/Level/EnemyPerLevelPhase",
        order = 1
    )]
    public class EnemyPerLevelPhase : ScriptableObject
    {
        public int phase;
        public Enemy[] enemies;
        public int maxNumberOfEnemies;
    }
}
