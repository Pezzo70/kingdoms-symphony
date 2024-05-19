using UnityEngine;
using Kingdom.Enums;
using Kingdom.Enums.MusicTheory;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/KeySignature", fileName = "KeySignature")]
    public class KeySignatureScriptable : ScriptableObject, ISprite
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private KeySignature keySignature;

        public Sprite Sprite { get => sprite; private set => sprite = value; }
        public KeySignature KeySignature{ get => keySignature;}
    }
}

