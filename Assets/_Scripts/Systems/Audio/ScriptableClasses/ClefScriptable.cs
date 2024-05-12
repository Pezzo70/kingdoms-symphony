using System.Collections;
using System.Collections.Generic;
using Kingdom.Enums.MusicTheory;
using UnityEngine;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Clef", fileName = "Clef")]
    public class ClefScriptable : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Clef clef;

        public Sprite Sprite { get => sprite; private set => sprite = value; }
        public Clef Clef { get => clef; private set => clef = value; }
    }
}