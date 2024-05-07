using Enums;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/End", fileName = "EndAudio")]
    public class EndGameAudio : ScriptableAudio
    {
        public EndState endState;
    }
}
