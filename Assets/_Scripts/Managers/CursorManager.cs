using UnityEditor;
using UnityEngine;

public static class CursorManager
{
    private static Texture2D _hoverCursor;
    private static Texture2D _clickCursor;
    private static Texture2D _lockedCursor;
    private static KingdomCursor _currentCursor;

    static CursorManager()
    {
        _hoverCursor = Resources.Load<Texture2D>("cursor_2");
        _clickCursor = Resources.Load<Texture2D>("cursor_3");
        _lockedCursor = Resources.Load<Texture2D>("cursor_4");
        _currentCursor = KingdomCursor.Default;
    }

    public static void SetCursor(KingdomCursor kingdomCursor)
    {
        _currentCursor = kingdomCursor;
        switch (kingdomCursor)
        {
            case KingdomCursor.Hover:
                Cursor.SetCursor(_hoverCursor, Vector2.zero, CursorMode.Auto);
                break;
            case KingdomCursor.Click:
                Cursor.SetCursor(_clickCursor, Vector2.zero, CursorMode.Auto);
                break;
            case KingdomCursor.Locked:
                Cursor.SetCursor(_lockedCursor, Vector2.zero, CursorMode.Auto);
                break;
            case KingdomCursor.Default:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }

    public static KingdomCursor GetCursor() => _currentCursor;

    public enum KingdomCursor
    {
        Default,
        Hover,
        Click,
        Locked
    }
}
