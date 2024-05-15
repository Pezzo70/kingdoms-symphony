using System;
using Kingdom.Enemies;
using Kingdom.Enums.Enemies;
using UnityEngine;

namespace Kingdom.Enums.Tips
{
    [CreateAssetMenu(fileName = "Tip", menuName = "ScriptableObjects/Tips", order = 1)]
    public class Tip : ScriptableObject
    {
        public TipID tipID;
        public bool isEnemy;
        public EnemyID enemyID;
    }
}
