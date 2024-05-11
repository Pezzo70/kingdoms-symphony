using System.Linq;
using Enums;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CursorManager;

[RequireComponent(typeof(Selectable))]
[AddComponentMenu("")]
public class UIEventTrigger : EventTrigger
{
    Animator fadeController;
    Selectable selectable;
    [SerializeField]
    private UIAction[] supportedActionsAudio = new UIAction[] { UIAction.Hover, UIAction.Submit, UIAction.Pause, UIAction.Cancel, UIAction.PopUp, UIAction.Return};


    public bool hasOpenBehaviour;
    public bool hasAudio = true;


    void Start()
    {
        TryGetComponent(out fadeController);
        selectable = GetComponent<Selectable>();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        if (!selectable.interactable)
            return;

        ExecuteUIAudio(UIAction.Hover);
        FadeIn();
        SetCursor(KingdomCursor.Hover);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        if (!selectable.interactable)
            return;

        FadeOut();
        SetCursor(KingdomCursor.Default);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        if (!selectable.interactable)
            return;
         
        SetCursor(KingdomCursor.Click);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        if (!selectable.interactable)
            return;


        if (GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;


        ExecuteUIAudio(UIAction.Submit);
        base.OnPointerClick(eventData);
    }

    public void OnDisable()
    {
        SetCursor(KingdomCursor.Default);
        FadeOut();
    }

    public void FadeOut() => fadeController?.Play("Fade_Out");

    public void FadeIn() => fadeController?.Play("Fade_In");

    private void ExecuteUIAudio(UIAction action)
    {
        if(!hasAudio || !supportedActionsAudio.Any(a => a.Equals(action))) return;
        AudioSystem audio = AudioSystem.Instance;
        switch (action)
        {
            case UIAction.Hover:
            case UIAction.Cancel:
            case UIAction.Pause:
            case UIAction.Return:
            case UIAction.PopUp:
                audio.Play(action);
                break;
            case UIAction.Submit:
                if(hasOpenBehaviour) audio.Play(UIAction.PopUp);
                else audio.Play(action);
                break;

        }
    }
}
