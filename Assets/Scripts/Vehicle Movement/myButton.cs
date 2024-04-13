using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class myButton : MonoBehaviour
{
    public bool isPressed;
    
    public float dampenPress = 0;
    public float buttonSensitivity = 2f;
    
    void Start()
    {
        setButton();
    }

    private void Update()
    {
        // check if the button has been pressed, then increase the sensitivity
        if (isPressed)
        {
            dampenPress += buttonSensitivity * Time.deltaTime;
        }
        else
        {
            dampenPress -= buttonSensitivity * Time.deltaTime;
        }
        dampenPress = Mathf.Clamp01(dampenPress);
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
