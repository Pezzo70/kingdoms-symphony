using Kingdom.Enums;
using UnityEngine;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/UI", fileName = "UIAudio")]
    public class UIAudio : ScriptableAudio
    {
        [SerializeField]
        private UIAction action;
        public UIAction Action => action;
    }
}
