using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class myButton : MonoBehaviour
{
    public bool isPressed;

    void Start()
    {
        setButton();
    }

    void setButton()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => onClickDown());
        
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => onClickUp());
        
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    void onClickDown()
    {
        isPressed = true;
    }

    void onClickUp()
    {
        isPressed = false;
    }
}
