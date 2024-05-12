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
            objectTag = data.pointerCurrentRaycast.gameObject.tag;
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
                    musicSheet.InsertSprite();
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
            };
        }

        public override void OnPointerEnter(PointerEventData data)
        {
            objectTag = data.pointerEnter.tag;
            switch (objectTag)
            {
                case "MusicSheet":
                    musicSheet.SetHover(true);
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
    }
}