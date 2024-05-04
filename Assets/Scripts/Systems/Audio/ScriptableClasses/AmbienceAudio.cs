using Enums;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Ambience", fileName = "AmbienceAudio")]
    public class AmbienceAudio : ScriptableAudio
    {
        [SerializeField] private Stages stage;
        public Stages Stages => stage;
    }
}