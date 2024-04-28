using UnityEditor;
using UnityEngine;

public static class CursorManager
{
    private static Texture2D hoverCursor;
    private static Texture2D clickCursor;
    private static KingdomCursor currentCursor;

    static CursorManager()
    {
        hoverCursor = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/cursor_2.png");
        clickCursor = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/cursor_3.png");
        currentCursor = KingdomCursor.Default;
    }

    public static void SetCursor(KingdomCursor kingdomCursor)
    {
        currentCursor = kingdomCursor;
        switch (kingdomCursor)
        {
            case KingdomCursor.Hover:
                Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
                break;
            case KingdomCursor.Click:
                Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
                break;
            case KingdomCursor.Default:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }

    public static KingdomCursor GetCursor() => currentCursor;

    public enum KingdomCursor
    {
        Default,
        Hover,
        Click
    }
}
