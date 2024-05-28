using Kingdom.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Kingdom.Audio
{
    public class MusicSheetInputHandler : EventTrigger
    {
        private MusicSheet musicSheet;

        private string objectTag;
        private bool isHovering;

        void Start()
        {
            musicSheet = gameObject.GetComponentInParent<MusicSheet>();
            objectTag = gameObject.tag;
        }

        public override void OnPointerClick(PointerEventData data)
        {
            var currentObject = data.pointerCurrentRaycast.gameObject;
            objectTag = currentObject.tag;
            switch (objectTag)
            {
                case "ChangeScale":
                    musicSheet.ChangeScale();
                    break;
            }
        }

        public void OnUndo(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.Undo();
        }

        public void OnClear(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.Clear();
        }

        public void OnAdd(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.CreatePage();
        }

        public void OnRemove(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.RemovePage();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.NavigatePage(true);
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.performed)
                musicSheet.NavigatePage(false);
        }

        public void OnNoteInsert(BaseEventData baseEventData)
        {
            if (baseEventData is PointerEventData point)
                if (point.button is PointerEventData.InputButton.Left)
                    musicSheet.InsertNote();
        }

        public void OnSignatureInsert(BaseEventData baseEventData)
        {
            if (baseEventData is PointerEventData point && point.button is PointerEventData.InputButton.Left)
                musicSheet.InsertKeySignature();
        }
    }
}
