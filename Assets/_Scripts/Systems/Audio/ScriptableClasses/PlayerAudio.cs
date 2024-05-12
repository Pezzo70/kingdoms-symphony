using Kingdom.Enums;
using Kingdom.Enums.Player;
using UnityEngine;

namespace Kingdom.Audio
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player", fileName = "PlayerAudio")]
    public class PlayerAudio : ScriptableAudio
    {
        private CharacterID player;
        [SerializeField]
        private ActorAudioTypes audioType;
        public  CharacterID Player => player;
        public ActorAudioTypes AudioType => audioType;
    }
}