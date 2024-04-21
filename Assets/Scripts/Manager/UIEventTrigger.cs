using UnityEngine;
using UnityEngine.EventSystems;
using static CursorManager;

public class UIEventTrigger : EventTrigger
{
    Animator FadeController;

    void Start() {
        TryGetComponent(out FadeController);
    }
    public override void OnPointerEnter(PointerEventData data) {
        FadeController?.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }
    public override void OnPointerExit(PointerEventData data){
        FadeController?.Play("Fade_Out");
        SetCursor(KingdomCursor.Default);
    }     

    public override void OnPointerDown(PointerEventData data){
        SetCursor(KingdomCursor.Click);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        if(GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);
    }
}
