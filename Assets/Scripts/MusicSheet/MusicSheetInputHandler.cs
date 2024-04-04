using System;
using UnityEngine;

public class MusicSheetInputHandler:MonoBehaviour 
{
    private MusicSheet parent;

    private String objectTag;

    void Start()
    {
        parent = gameObject.GetComponentInParent<MusicSheet>();
        objectTag = gameObject.tag;
    }

    void OnMouseDown()
    { 
        switch (objectTag)
        {
            case "Undo":
                 parent.Undo();
                 break;
            case "Clear":
                  parent.Clear();
                  break;
            case "ChangeScale":
                  parent.ChangeScale();
                  break;
        };
    }
}