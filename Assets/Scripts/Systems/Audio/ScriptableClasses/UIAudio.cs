using Enums;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/UI", fileName = "UIAudio")]
    public class UIAudio : ScriptableAudio
    {
        [SerializeField]
        private UIAction action;
        public UIAction Action => action;
    }
}
