using Enemies;
using Enums;
using UnityEngine;

namespace Audio
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Enemy", fileName = "EnemyAudio")]
    public class EnemyAudio : ScriptableAudio
    {
        [SerializeField]
        private EnemyID enemy;
        [SerializeField]
        private ActorAudioTypes audioType;
        public  EnemyID Enemy => enemy;
        public ActorAudioTypes AudioType => audioType;
    }
}