using UnityEngine;

namespace Kingdom.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level/Level", order = 0)]
    public class Level : ScriptableObject
    {
        public int sceneID;
        public int numberOfPhases;
        public EnemyPerLevelPhase[] enemiesPerLevel;
        public bool endGame;
    }
}
