using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Used to tell if mouse is over a certain UI element.
public class ClickThroughPreventer : EventTrigger
{
    public bool mouseOver = false;
    public override void OnPointerEnter(PointerEventData data)
    {
        mouseOver = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        mouseOver = false;
    }
    
    public bool IsMouseOver()
    {
        return mouseOver;
    }
    
}
