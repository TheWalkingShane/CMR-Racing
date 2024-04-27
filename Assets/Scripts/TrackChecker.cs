using UnityEngine;
using UnityEngine.UI; // Namespace for UI elements like Image

public class TrackChecker : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask floorMask;
    public Transform carTransform;
    public Image trackStateImage;
    public Sprite onTrackSprite, offTrackSprite, edgeOfTrackSprite, speedIncreaseSprite, speedDecreaseSprite;

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

 private const int tolerance = 10; // Increase the tolerance to see if it helps.


    // Target RGB colors as class-level variables
    private Color targetBlue = new Color(6 / 255f, 0 / 255f, 254 / 255f);
    private Color targetGreen = new Color(113 / 255f, 253 / 255f, 144 / 255f);
    private Color targetRed = new Color(255 / 255f, 114 / 255f, 113 / 255f);
    private Color targetLightBlue = new Color(115 / 255f, 163 / 255f, 255 / 255f); // Light blue for EdgeOfTrack

    void Start()
    {
        CheckComponentAssignments();
    }

    void Update()
    {
        DetectTrackState();
    }

    private void CheckComponentAssignments()
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

    private void DetectTrackState()
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
        if (IsBlueChannelMatch(color))
            return TrackState.OnTrack;
        if (IsLightBlueChannelMatch(color))
            return TrackState.EdgeOfTrack;
        if (IsGreenChannelMatch(color))
            return TrackState.SpeedIncrease;
        if (IsRedChannelMatch(color))
            return TrackState.SpeedDecrease;
        return TrackState.OffTrack;
    }

    private bool IsBlueChannelMatch(Color color)
    {
        return Mathf.Abs(color.b - targetBlue.b) <= tolerance / 255f;
    }

    private bool IsGreenChannelMatch(Color color)
    {
        return Mathf.Abs(color.g - targetGreen.g) <= tolerance / 255f;
    }

    private bool IsRedChannelMatch(Color color)
    {
        return Mathf.Abs(color.r - targetRed.r) <= tolerance / 255f;
    }

    private bool IsLightBlueChannelMatch(Color color)
{
    return Mathf.Abs(color.r - targetLightBlue.r) <= tolerance / 255f &&
           Mathf.Abs(color.g - targetLightBlue.g) <= tolerance / 255f &&
           Mathf.Abs(color.b - targetLightBlue.b) <= tolerance / 255f;
}

    private void UpdateTrackStateUI(TrackState state)
    {
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
        trackStateImage.enabled = true; // Ensure the image is visible when needed
    }
}
