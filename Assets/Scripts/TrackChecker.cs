using UnityEngine;

public class TrackChecker : MonoBehaviour
{
    public Camera mainCamera; // Assign the camera used for raycasting in the Unity inspector.
    public LayerMask floorMask; // Assign the layer mask for the floor in the Unity inspector.
    public Transform carTransform; // Assign your car's Transform here in the Unity inspector.
    public string floorObjectName = "Floor"; // Set this to the name of your floor object in the scene.

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
        // Check if the main camera is assigned
        if (mainCamera == null)
        {
            Debug.LogError("TrackChecker: Main Camera is not assigned in the inspector.");
            return;
        }

        // Find the floor object by name
        GameObject floorObject = GameObject.Find(floorObjectName);
        if (floorObject == null)
        {
            Debug.LogError("TrackChecker: No floor object found with name: " + floorObjectName);
            return;
        }

        // Get the Renderer component from the floor object
        floorRenderer = floorObject.GetComponent<Renderer>();
        if (floorRenderer == null)
        {
            Debug.LogError("TrackChecker: No Renderer component found on floor object.");
            return;
        }

        // Get the Texture from the Renderer component
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

                switch (state)
                {
                    case TrackState.OnTrack:
                        Debug.Log("TrackChecker: On track");
                        // Implement logic for being on track
                        break;
                    case TrackState.EdgeOfTrack:
                        Debug.Log("TrackChecker: Edge of track");
                        // Implement logic for being on the edge of the track
                        break;
                    case TrackState.OffTrack:
                        Debug.Log("TrackChecker: Off track");
                        // Implement logic for being off track
                        break;
                    case TrackState.SpeedIncrease:
                        Debug.Log("TrackChecker: Speed increase");
                        // Implement logic for speed increase
                        break;
                    case TrackState.SpeedDecrease:
                        Debug.Log("TrackChecker: Speed decrease");
                        // Implement logic for speed decrease
                        break;
                }
            }
        }
    }

    private TrackState GetTrackState(Color color)
    {
        // Convert from float [0,1] to int [0,255]
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);

        // Define specific RGB values for different track states
        if (r == 6 && g == 0 && b == 254) // On track (blue)
        {
            return TrackState.OnTrack;
        }
        else if (r == 115 && g == 163 && b == 255) // Edge of track (light blue)
        {
            return TrackState.EdgeOfTrack;
        }
        else if (r == 113 && g == 253 && b == 144) // Speed increase (green)
        {
            return TrackState.SpeedIncrease;
        }
        else if (r == 255 && g == 114 && b == 113) // Speed decrease (red)
        {
            return TrackState.SpeedDecrease;
        }
        else if (r == 145 && g == 154 && b == 149) // Off track (grey)
        {
            return TrackState.OffTrack;
        }
        else
        {
            return TrackState.OffTrack; // Default to off track if none match
        }
    }
}

