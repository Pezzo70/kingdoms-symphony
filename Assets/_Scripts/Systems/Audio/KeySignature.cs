using UnityEngine;



namespace Kingdom.Audio
{
    public class KeySignature : MonoBehaviour
    {

        public KeySignatureScriptable keySignature;
        public int line;
        public int page;

        public override string ToString()
        {
            return $"KEYSIGNATURE {keySignature.KeySignature} - LINE {line} / PAGE {page}";
        }
    }
}