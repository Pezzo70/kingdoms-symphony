using Kingdom.Audio;
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
                case "Undo":
                    musicSheet.Undo();
                    break;
                case "Clear":
                    musicSheet.Clear();
                    break;
                case "ChangeScale":
                    musicSheet.ChangeScale();
                    break;
                case "MusicSheet":
                    musicSheet.InsertNote();
                    break;
                case "Next":
                    musicSheet.NavigatePage(true);
                    break;
                case "Previous":
                    musicSheet.NavigatePage(false);
                    break;
                case "Add":
                    musicSheet.CreatePage();
                    break;
                case "Remove":
                    musicSheet.RemovePage();
                    break;
                case "Play":
                    musicSheet.Play();
                    break;
                case "KeySignatureArea":
                    musicSheet.InsertKeySignature();
                    break;
                case "Note":
                    if(data.button is PointerEventData.InputButton.Left)
                        musicSheet.ChangeNote(currentObject.transform.GetComponent<Note>());
                    else if(data.button is PointerEventData.InputButton.Right)
                        musicSheet.RemoveNote(currentObject.transform.GetComponent<Note>());
                    break;
            };
        }

        public override void OnPointerEnter(PointerEventData data)
        {
            objectTag = data.pointerEnter.tag;
            switch (objectTag)
            {
                case "MusicSheet":
                case "KeySignatureArea":
                    musicSheet.SetHover(true, objectTag);
                    isHovering = true;
                    break;
            };
        }

        public override void OnPointerExit(PointerEventData data)
        {
            isHovering = false;
            musicSheet.SetHover(isHovering);
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
    }
}