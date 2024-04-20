using System.Collections;
using UnityEditor;
using UnityEngine;
using static CursorManager;

public class UIFadeController : MonoBehaviour
{
    Animator FadeController;

    void Start() {
        string hoverPath = AssetDatabase.GUIDToAssetPath("04cab998fb0f9674095ac9817239afee");
        string clickPath = AssetDatabase.GUIDToAssetPath("0b5f125993ed66f443b755adfb5d71089");

        FadeController = GetComponent<Animator>();
    }
    void OnMouseEnter() {
        FadeController.Play("Fade_In");
        SetCursor(KingdomCursor.Hover);
    }
    void OnMouseExit() {
        FadeController.Play("Fade_Out");
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
