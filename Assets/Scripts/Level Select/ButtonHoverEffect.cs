using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public float jumpHeight = 10f;
    public float jumpSpeed = 5f;
    private bool isHovering = false;

    private RectTransform buttonRectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        
        buttonRectTransform = GetComponent<RectTransform>();
        
        originalPosition = buttonRectTransform.localPosition;
    }

    void Update()
    {
        
        if (isHovering)
        {
            
            float newY = originalPosition.y + Mathf.Sin(Time.time * jumpSpeed) * jumpHeight;
            
            buttonRectTransform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
        else
        {
            
            buttonRectTransform.localPosition = originalPosition;
        }
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}