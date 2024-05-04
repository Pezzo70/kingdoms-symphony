using UnityEngine;
namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/BaseAudio", fileName = "Audio")]
    public class ScriptableAudio : ScriptableObject
    {
        [SerializeField] AudioClip audioClip;
        public AudioClip AudioClip => audioClip;
    }

}