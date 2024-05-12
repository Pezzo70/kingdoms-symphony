using UnityEngine;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Stage", fileName = "StageAudio")]
    public class StageAudio : ScriptableAudio
    {
        [SerializeField] 
        private string stage;
        public string StageName => stage;
    }
}