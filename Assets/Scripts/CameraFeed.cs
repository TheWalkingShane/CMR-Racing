using UnityEngine;

public class CameraFeed : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    public Renderer displayRenderer;

    public Color targetColor = Color.red; // Color to detect
    public float colorThreshold = 100f;   // Color detection threshold

    void Start()
    {
        Debug.Log("CameraFeed script started.");

        // Check if the displayRenderer is assigned
        if (displayRenderer == null)
        {
            Debug.LogError("Display renderer not assigned. Camera feed will not render.");
            return;
        }

        // Initialize and start the webcam texture
        webcamTexture = new WebCamTexture();
        displayRenderer.material.mainTexture = webcamTexture;

        Debug.Log("Webcam resolution: " + webcamTexture.width + "x" + webcamTexture.height);

        webcamTexture.Play();
    }

    void Update()
    {
        // Check if the webcam texture is playing
        if (!webcamTexture.isPlaying)
        {
            Debug.LogWarning("Webcam not playing. Attempting to start...");
            webcamTexture.Play();
        }

        // Perform color detection on webcam texture frames
        if (webcamTexture.isPlaying && webcamTexture.didUpdateThisFrame)
        {
            Color[] pixels = webcamTexture.GetPixels();

            // Loop through each pixel in the webcam texture
            foreach (Color pixel in pixels)
            {
                // Calculate the color difference using Euclidean distance in RGB space
                float rDiff = pixel.r - targetColor.r;
                float gDiff = pixel.g - targetColor.g;
                float bDiff = pixel.b - targetColor.b;
                float colorDifference = Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);

                // If the color difference is less than the threshold, the color is detected
                if (colorDifference < colorThreshold)
                {
                    Debug.Log("Color detected!");
                    // You can add more actions here based on color detection
                }
            }
        }
    }

    void OnDestroy()
    {
        // Clean up the webcam texture when the script is destroyed
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
            Debug.Log("Webcam texture stopped.");
        }
    }
}