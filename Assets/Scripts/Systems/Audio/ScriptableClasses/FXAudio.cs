using FX;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/FX", fileName = "FXAudio")]
    public class FXAudio : ScriptableAudio
    {
        public FXID fxID;
        public FXState fxState;
    }
}
