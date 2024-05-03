using System.ComponentModel;
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

        [Range(0, 100)]
        public int probability;

        [Header(
            "Factors (Applied on Scroll effect) - Must check GDD\nPercentage values does not use the 0-1 floating point interval."
        )]
        public float xFactor;
        public float yFactor;
        public float zFactor;
        public float wFactor;
        public float jFactor;
        public float mFactor;
        public float lFactor;
    }
}
