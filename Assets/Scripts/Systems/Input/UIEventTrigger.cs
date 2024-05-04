using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CursorManager;

[RequireComponent(typeof(Selectable))]
public class UIEventTrigger : EventTrigger
{
    Animator fadeController;
    Selectable selectable;
    public bool isSelected;

    void Start() {
        TryGetComponent(out fadeController);
        selectable = GetComponent<Selectable>();


        if(isSelected) 
            this.FadeIn();
    }

    public override void OnPointerEnter(PointerEventData data) {
        if(!selectable.interactable) 
            return;

        fadeController?.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }
    public override void OnPointerExit(PointerEventData data){
        if(!selectable.interactable) 
            return;
        if(!isSelected) 
            FadeOut();

        SetCursor(KingdomCursor.Default);
    }     

    public override void OnPointerDown(PointerEventData data){
        if(!selectable.interactable) 
            return;

        SetCursor(KingdomCursor.Click);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        if(!selectable.interactable) 
            return;

        if(GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;

        
        this.OnSubmit(eventData);
        base.OnPointerClick(eventData);
    }

    public void OnDisable()
    {
        SetCursor(KingdomCursor.Default);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        this.FadeOut();
        isSelected = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        this.FadeIn();
        isSelected = true;
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
    }

    public void FadeOut() => fadeController?.Play("Fade_Out");
    public void FadeIn() => fadeController?.Play("Fade_In");
}
