using Kingdom.Enums.FX;
using UnityEngine;

namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/FX", fileName = "FXAudio")]
    public class FXAudio : ScriptableAudio
    {
        public FXID fxID;
        public FXState fxState;
    }
}
