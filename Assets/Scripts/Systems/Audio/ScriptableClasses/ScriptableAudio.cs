using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/BaseAudio", fileName = "Audio")]
    public class ScriptableAudio : ScriptableObject, IWeightable
    {
        [SerializeField]
        AudioClip audioClip;
        public AudioClip AudioClip => audioClip;

        [SerializeField, Range(0f, 1f)]
        float weight = 1f;
        public float Weight
        {
            get => weight;
            set => weight = value;
        }

        [Range(0, 1)]
        public float volume = 1f;
    }
}
