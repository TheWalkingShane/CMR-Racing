using UnityEngine;
using UnityEngine.UI; // Namespace for UI elements like Image

public class TrackChecker : MonoBehaviour
{
    public Camera mainCamera; // Assign the camera used for raycasting in the Unity inspector.
    public LayerMask floorMask; // Assign the layer mask for the floor in the Unity inspector.
    public Transform carTransform; // Assign your car's Transform here in the Unity inspector.
    public Image trackStateImage; // UI Image component to show track state
    public Sprite onTrackSprite;  // Sprite for being on track
    public Sprite offTrackSprite; // Sprite for being off track
    public Sprite edgeOfTrackSprite; // Sprite for edge of track
    public Sprite speedIncreaseSprite; // Sprite for speed increase (boost)
    public Sprite speedDecreaseSprite; // Sprite for speed decrease (slow)

    private Renderer floorRenderer;
    private Texture2D trackTexture;

    private enum TrackState
    {
        OnTrack,
        EdgeOfTrack,
        OffTrack,
        SpeedIncrease,
        SpeedDecrease
    }

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("TrackChecker: Main Camera is not assigned in the inspector.");
            return;
        }

        GameObject floorObject = GameObject.Find("Floor");
        if (floorObject == null)
        {
            Debug.LogError("TrackChecker: No floor object found with the specified name.");
            return;
        }

        floorRenderer = floorObject.GetComponent<Renderer>();
        if (floorRenderer == null)
        {
            Debug.LogError("TrackChecker: No Renderer component found on floor object.");
            return;
        }

        trackTexture = floorRenderer.material.mainTexture as Texture2D;
        if (trackTexture == null)
        {
            Debug.LogError("TrackChecker: No Texture2D found on floor object's material.");
        }
    }

    void Update()
    {
        if (mainCamera != null && carTransform != null && trackTexture != null)
        {
            Ray ray = new Ray(carTransform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask))
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= trackTexture.width;
                pixelUV.y *= trackTexture.height;

                Color pixelColor = trackTexture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                TrackState state = GetTrackState(pixelColor);

                UpdateTrackStateUI(state);
            }
        }
    }

    private TrackState GetTrackState(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);

        int tolerance = 10; // Tolerance for color matching
        if (Mathf.Abs(r - 6) <= tolerance && Mathf.Abs(g - 0) <= tolerance && Mathf.Abs(b - 254) <= tolerance)
        {
            return TrackState.OnTrack;
        }
        if (Mathf.Abs(r - 115) <= tolerance && Mathf.Abs(g - 163) <= tolerance && Mathf.Abs(b - 255) <= tolerance)
        {
            return TrackState.EdgeOfTrack;
        }
        if (Mathf.Abs(r - 113) <= tolerance && Mathf.Abs(g - 253) <= tolerance && Mathf.Abs(b - 144) <= tolerance)
        {
            return TrackState.SpeedIncrease;
        }
        if (Mathf.Abs(r - 255) <= tolerance && Mathf.Abs(g - 114) <= tolerance && Mathf.Abs(b - 113) <= tolerance)
        {
            return TrackState.SpeedDecrease;
        }
        if (Mathf.Abs(r - 145) <= tolerance && Mathf.Abs(g - 154) <= tolerance && Mathf.Abs(b - 149) <= tolerance)
        {
            return TrackState.OffTrack;
        }
        return TrackState.OffTrack;
    }

    private void UpdateTrackStateUI(TrackState state)
    {
        if (trackStateImage == null) return; // Ensure the image component is assigned.

        switch (state)
        {
            case TrackState.OnTrack:
                trackStateImage.sprite = onTrackSprite;
                break;
            case TrackState.EdgeOfTrack:
                trackStateImage.sprite = edgeOfTrackSprite;
                break;
            case TrackState.OffTrack:
                trackStateImage.sprite = offTrackSprite;
                break;
            case TrackState.SpeedIncrease:
                trackStateImage.sprite = speedIncreaseSprite;
                break;
            case TrackState.SpeedDecrease:
                trackStateImage.sprite = speedDecreaseSprite;
                  break;
        default:
            trackStateImage.enabled = false; // Hide the image if the state is not recognized
            break;
    }
    trackStateImage.enabled = true; // Show the image for any recognized state
}
        }
