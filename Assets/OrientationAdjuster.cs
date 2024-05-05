using UnityEngine;
using UnityEngine.UI;

public class OrientationAdjuster : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        UpdateCanvasScaler();
    }

    void Update()
    {
        UpdateCanvasScaler();
    }

    private void UpdateCanvasScaler()
    {
        if (Screen.height > Screen.width) // Portrait
        {
            canvasScaler.referenceResolution = new Vector2(1080, 2448);
        }
        else // Landscape
        {
            canvasScaler.referenceResolution = new Vector2(2448, 1080);
        }
    }
}