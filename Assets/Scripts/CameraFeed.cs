using UnityEngine;

public class CameraFeed : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    public Renderer displayRenderer;

    public Color targetColorRed = Color.red;   // Red color to detect
    public Color targetColorGreen = Color.green; // Green color to detect
    public float colorThreshold = 100f;         // Color detection threshold
    public GameObject bombPowerUpPrefab;        // Bomb prefab to spawn
    public GameObject greenPowerUpPrefab;       // Speed power-up prefab to spawn
    public GameObject spawnObject;              
    public float xOffset = 1f;                  
    public float yOffset = 0f;                  
    public float zOffset = 1f;                 
    public float detectionCooldown = 2f;        
    private float lastDetectionTime = 0f;       

    void Start()
    {
        Debug.Log("CameraFeed script started.");

       
        if (displayRenderer == null)
        {
            Debug.LogError("Display renderer not assigned. Camera feed will not render.");
            return;
        }

        
        webcamTexture = new WebCamTexture();
        displayRenderer.material.mainTexture = webcamTexture;

        Debug.Log("Webcam resolution: " + webcamTexture.width + "x" + webcamTexture.height);

        webcamTexture.Play();
    }

    void Update()
    {
        
        if (!webcamTexture.isPlaying)
        {
            Debug.LogWarning("Webcam not playing. Attempting to start...");
            webcamTexture.Play();
        }

        // Perform color detection on webcam texture frames
        if (webcamTexture.isPlaying && webcamTexture.didUpdateThisFrame && Time.time - lastDetectionTime >= detectionCooldown)
        {
            Color[] pixels = webcamTexture.GetPixels();

            // Loop through each pixel in the webcam texture
            foreach (Color pixel in pixels)
            {
                // Calculate the color difference for red and green colors
                float redDifference = ColorDifference(pixel, targetColorRed);
                float greenDifference = ColorDifference(pixel, targetColorGreen);

                // Check if the pixel color is similar to red within the threshold
                if (redDifference < colorThreshold)
                {
                    Debug.Log("Red color detected!");

                    // Spawn the bomb power-up with an offset from the spawn object
                    SpawnPowerUp(bombPowerUpPrefab);
                    lastDetectionTime = Time.time;
                    break; // Exit the loop once red color is detected
                }
                // Check if the pixel color is similar to green within the threshold
                else if (greenDifference < colorThreshold)
                {
                    Debug.Log("Green color detected!");

                    // Spawn the speed power-up with an offset from the spawn object
                    SpawnPowerUp(greenPowerUpPrefab);
                    lastDetectionTime = Time.time;
                    break; // Exit the loop once green color is detected
                }
            }
        }
    }

    void OnDestroy()
    {
        
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
            Debug.Log("Webcam texture stopped.");
        }
    }

    private float ColorDifference(Color color1, Color targetColor)
    {
        float dR = color1.r - targetColor.r;
        float dG = color1.g - targetColor.g;
        float dB = color1.b - targetColor.b;
        return dR * dR + dG * dG + dB * dB;
    }

    void SpawnPowerUp(GameObject powerUpPrefab)
    {
        if (spawnObject != null && powerUpPrefab != null)
        {
            // Get the position of the spawnObject
            Vector3 spawnPosition = spawnObject.transform.position;

          
            spawnPosition += spawnObject.transform.forward * zOffset;
            spawnPosition += spawnObject.transform.right * xOffset;
            spawnPosition += spawnObject.transform.up * yOffset;

            
            Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Spawn object or power-up prefab not assigned.");
        }
    }
}