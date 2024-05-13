using System;
using Kingdom.Enemies;
using UnityEditor;
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

    [CustomEditor(typeof(Tip))]
    public class TipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var tip = target as Tip;

            tip.tipID = (TipID)EditorGUILayout.EnumPopup("Tip ID:", tip.tipID);

            tip.isEnemy = GUILayout.Toggle(tip.isEnemy, "Is Enemy");

            if (tip.isEnemy)
                tip.enemyID = (EnemyID)EditorGUILayout.EnumPopup("Enemy ID:", tip.enemyID);
        }
    }
}
