using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemies/Enemy", order = 1)]
    public class Enemy : ScriptableObject
    {
        public EnemyID enemyID;
        public int moral;
        public int mana;
        public EnemyAttack[] attacks;
        public EnemyAdvantage[] advantages;
        public EnemyDisadvantage[] disadvantages;
        public int XP;
    }
}