using UnityEngine;

public class TrackChecker : MonoBehaviour
{
    public Camera mainCamera; // Assign the camera used for raycasting in the Unity inspector.
    public LayerMask floorMask; // Assign the layer mask for the floor in the Unity inspector.
    public Transform carTransform; // Assign your car's Transform here in the Unity inspector.
    public string floorObjectName = "Floor"; // Set this to the name of your floor object in the scene.

    private Renderer floorRenderer;
    private Texture2D trackTexture;

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
        // Ensure that the camera, carTransform, and track texture are assigned before proceeding
        if (mainCamera != null && carTransform != null && trackTexture != null)
        {
            // Get the car's position on screen
            Vector3 carScreenPosition = mainCamera.WorldToScreenPoint(carTransform.position);
            Ray ray = mainCamera.ScreenPointToRay(carScreenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask))
            {
                // Convert hit point to texture space
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= trackTexture.width;
                pixelUV.y *= trackTexture.height;

                // Check pixel color
                Color pixelColor = trackTexture.GetPixel((int)pixelUV.x, (int)pixelUV.y);

                if (IsTrack(pixelColor))
                {
                    Debug.Log("TrackChecker: On track");
                }
                else
                {
                    Debug.Log("TrackChecker: Off track");
                }
            }
        }
    }

    // Determines if the color of the pixel corresponds to the track
    private bool IsTrack(Color color)
    {
        // Define pink color range
        Color lowerPink = new Color(0.8f, 0.6f, 0.9f); // These are RGB values as fractions of 255
        Color upperPink = new Color(1.0f, 0.8f, 1.0f); // Adjust these values based on your actual track color

        // Check if color is within the pink color range
        return color.r >= lowerPink.r && color.r <= upperPink.r
               && color.g >= lowerPink.g && color.g <= upperPink.g
               && color.b >= lowerPink.b && color.b <= upperPink.b;
    }
}
