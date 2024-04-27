using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicSheetInputHandler:EventTrigger 
{
    private MusicSheet parent;

    private String objectTag;

    private bool isHovering;

    void Start()
    {
        parent = gameObject.GetComponentInParent<MusicSheet>();
        objectTag = gameObject.tag;
    }

     public override void OnPointerClick(PointerEventData data)
    { 
        objectTag = data.pointerCurrentRaycast.gameObject.tag;
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
            case "MusicSheet":
                  parent.InsertSprite();
                  break;
        };
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        objectTag = data.pointerEnter.tag;
        switch (objectTag)
        {
            case "MusicSheet":
                 parent.SetHover(true);
                 isHovering = true;
                 break;
        };
    }

    public override void OnPointerExit(PointerEventData data)
    {
        isHovering = false;
        parent.SetHover(isHovering);
    }
}