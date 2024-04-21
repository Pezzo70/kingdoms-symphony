using System.Collections;
using UnityEditor;
using UnityEngine;
using static CursorManager;

[RequireComponent(typeof(BoxCollider2D))]
public class UIFadeController : MonoBehaviour
{
    Animator FadeController;

    void Start() {
        TryGetComponent(out FadeController);
    }
    void OnMouseEnter() {
        FadeController?.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }
    void OnMouseExit() {
        FadeController?.Play("Fade_Out");
        SetCursor(KingdomCursor.Default);
    }     

    void OnMouseDown() {
        SetCursor(KingdomCursor.Click);
    }

    void OnMouseUp()
    {
        if(GetCursor() is KingdomCursor.Click)
            SetCursor(KingdomCursor.Hover);
    }
}
