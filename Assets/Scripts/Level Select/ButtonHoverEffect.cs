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

    public Image hoverImageDisplay;  // The Image component that displays the hover image
    public Sprite[] levelImages;  // The sprites for each level button

    void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        originalPosition = buttonRectTransform.localPosition;

        // Ensure the image is disabled at start
        hoverImageDisplay.enabled = false;
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
        hoverImageDisplay.enabled = true;  // Enable the Image component when hovered

        int index = -1;
        if (gameObject.name == "Level1")
            index = 0;
        else if (gameObject.name == "Level2")
            index = 1;
        else if (gameObject.name == "Level3")
            index = 2;

        if (index != -1 && index < levelImages.Length)
        {
            hoverImageDisplay.sprite = levelImages[index];
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        hoverImageDisplay.enabled = false;  // Disable the Image component when not hovered
        hoverImageDisplay.sprite = null;  // Optionally clear the sprite
    }
}