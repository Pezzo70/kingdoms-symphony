using UI;
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

    [field: SerializeField]
    public bool hasOpenBehaviour;

    void Start()
    {
        TryGetComponent(out fadeController);
        selectable = GetComponent<Selectable>();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        if (!selectable.interactable)
            return;

        AudioSystem.Instance.Play(UIAction.Hover);
        fadeController?.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        if (!selectable.interactable)
            return;

        SetCursor(KingdomCursor.Default);
        FadeOut();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        if (!selectable.interactable)
            return;

        SetCursor(KingdomCursor.Click);
        AudioSystem.Instance.Play(UIAction.Submit);
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

        if (hasOpenBehaviour)
        {
            AudioSystem.Instance.Play(UI.UIAction.PopUp);
            FadeOut();
        }

        base.OnPointerClick(eventData);
    }

    public void OnDisable()
    {
        SetCursor(KingdomCursor.Default);
        FadeOut();
    }

    public void FadeOut() => fadeController?.Play("Fade_Out");

    public void FadeIn() => fadeController?.Play("Fade_In");
}
