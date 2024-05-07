using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    public Sprite prepLapSprite, lapOneSprite, lapTwoSprite, finalLapSprite;
    public Image lapImage; // Attach this via the Unity Inspector

    private void Start()
    {
        // Ensure the pre-lap sprite is set when the game starts
        Debug.Log("Game start - setting pre-lap sprite.");
        UpdateLapDisplay(0);
    }

    private void OnEnable()
    {
        // Subscribe to the OnLapChange event from CheckpointManager
        CheckpointManager.OnLapChange += UpdateLapDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnLapChange event to avoid memory leaks
        CheckpointManager.OnLapChange -= UpdateLapDisplay;
    }

    // This method handles the event and updates the display of lap sprites
    private void UpdateLapDisplay(int currentLap)
    {
        Debug.Log($"Updating display for lap {currentLap}");
        switch (currentLap)
        {
            case 0:
                if (prepLapSprite == null) Debug.Log("Pre-lap sprite is not assigned.");
                lapImage.sprite = prepLapSprite;
                Debug.Log("Displaying preparation lap sprite.");
                break;
            case 1:
                lapImage.sprite = lapOneSprite;
                Debug.Log("Switching to first lap sprite.");
                break;
            case 2:
                lapImage.sprite = lapTwoSprite;
                Debug.Log("Switching to second lap sprite.");
                break;
            case 3:
                lapImage.sprite = finalLapSprite;
                Debug.Log("Switching to final lap sprite.");
                break;
            default:
                Debug.Log("No sprite available for lap " + currentLap);
                break;
        }
    }
}
