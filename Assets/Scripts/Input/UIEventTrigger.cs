using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static CursorManager;

//[RequireComponent(typeof(Selectable))]
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
        if(!selectable?.interactable ?? true) 
            return;
        fadeController?.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }
    public override void OnPointerExit(PointerEventData data){
        if(!selectable?.interactable ?? true) 
            return;
        if(!isSelected) 
            FadeOut();

        SetCursor(KingdomCursor.Default);
    }     

    public override void OnPointerDown(PointerEventData data){
        if(!selectable?.interactable ?? true) 
            return;

        SetCursor(KingdomCursor.Click);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        if(!selectable?.interactable ?? true) 
            return;

        if(GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left && (!selectable?.interactable ?? true)) 
            return;

        selectable.Select();
        this.OnSubmit(eventData);
        base.OnPointerClick(eventData);
    }

    public void OnDisable()
    {
        SetCursor(KingdomCursor.Default);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if(!selectable?.interactable ?? true) 
            return;

        this.FadeOut();
        isSelected = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if(!selectable?.interactable ?? true) 
            return;

        this.FadeIn();
        isSelected = true;
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if(!selectable?.interactable ?? true) 
            return;

        base.OnSubmit(eventData);
    }

    public void FadeOut() => fadeController?.Play("Fade_Out");
    public void FadeIn() => fadeController?.Play("Fade_In");
}
