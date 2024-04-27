using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(
        fileName = "EnemyAttack",
        menuName = "ScriptableObjects/Enemies/EnemyAttack",
        order = 2
    )]
    public class EnemyAttack : ScriptableObject
    {
        public EnemyAttackID enemyAttackID;
        public AttackType attackType;
        public int manaRequired;
        public float probability;
        public float xFactor;
        public float yFactor;
        public float zFactor;
        public float wFactor;
        public float jFactor;
        public float kFactor;
    }
}
