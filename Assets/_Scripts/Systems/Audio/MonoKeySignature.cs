using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace Kingdom.Audio
{
    public class MonoKeySignature : UIEventTrigger
    {
        public MusicSheet musicSheet;
        public KeySignatureScriptable keySignature;
        public int line;
        public int page;

        public void Awake()
        {
            musicSheet = FindAnyObjectByType<MusicSheet>();
            this.AddComponent<Selectable>();
        }

        public override void OnPointerEnter(PointerEventData data)=> musicSheet.SetHover(1);

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button is PointerEventData.InputButton.Right)
               musicSheet.RemoveSignature(this);
            else if(eventData.button is PointerEventData.InputButton.Left)
                musicSheet.ChangeSignature(this);

            base.OnPointerClick(eventData);
        }
        public override string ToString()
        {
            return $"KEYSIGNATURE {keySignature.KeySignature} - LINE {line} / PAGE {page}";
        }
    }
}