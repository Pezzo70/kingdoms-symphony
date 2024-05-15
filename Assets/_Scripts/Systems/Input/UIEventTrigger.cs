using System.Collections;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CursorManager;

public class UIEventTrigger : EventTrigger
{
    protected Animator fadeController;
    protected Selectable selectable;
    private Coroutine _waitUserStayOnButton = null;

    [SerializeField]
    private UIAction[] supportedActionsAudio = new UIAction[]
    {
        UIAction.Hover,
        UIAction.Submit,
        UIAction.Pause,
        UIAction.Cancel,
        UIAction.PopUp,
        UIAction.Return
    };

    public bool hasOpenBehaviour;
    public bool hasAudio = true;

    void Start()
    {
        TryGetComponent(out fadeController);
        TryGetComponent(out selectable);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        if (!selectable?.interactable ?? false)
        {
            SetCursor(KingdomCursor.Locked);
            return;
        }

        _waitUserStayOnButton = StartCoroutine(WaitUserStayOnButton());

        base.OnPointerEnter(data);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        if (!selectable?.interactable ?? false)
        {
            if (GetCursor() is KingdomCursor.Locked)
                SetCursor(KingdomCursor.Default);

            return;
        }

        if (_waitUserStayOnButton == null)
        {
            FadeOut();
            SetCursor(KingdomCursor.Default);
        }
        else
        {
            StopAllCoroutines();
            _waitUserStayOnButton = null;
        }

        base.OnPointerExit(data);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        if (!selectable?.interactable ?? false)
            return;

        SetCursor(KingdomCursor.Click);

        base.OnPointerDown(data);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        if (!selectable?.interactable ?? false)
            return;

        if (GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);

        base.OnPointerUp(data);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!selectable?.interactable ?? false)
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
        if (!hasAudio || !supportedActionsAudio.Any(a => a.Equals(action)))
            return;
        AudioSystem audio = AudioSystem.Instance;
        switch (action)
        {
            case UIAction.Hover:
            case UIAction.Cancel:
            case UIAction.Pause:
            case UIAction.Return:
            case UIAction.PopUp:
            case UIAction.Submit:
                audio.Play(action);
                break;
        }
    }

    private IEnumerator WaitUserStayOnButton()
    {
        yield return new WaitForSeconds(0.15f);
        ExecuteUIAudio(UIAction.Hover);
        FadeIn();
        SetCursor(KingdomCursor.Hover);
        _waitUserStayOnButton = null;
    }
}
