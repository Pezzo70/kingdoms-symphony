using Kingdom.Enums;
using UnityEngine;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/End", fileName = "EndAudio")]
    public class EndGameAudio : ScriptableAudio
    {
        public EndState endState;
    }
}
