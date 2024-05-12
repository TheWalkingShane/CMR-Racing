using UnityEngine;

public class CameraFeed : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    public Renderer displayRenderer;

    public Color targetColorRed = Color.red;
    public Color targetColorGreen = Color.green;
    public float colorThreshold = 100f;
    public GameObject bombPowerUpPrefab;
    public GameObject greenPowerUpPrefab;
    public GameObject spawnObject;
    public float xOffset = 1f;
    public float yOffset = 0f;
    public float zOffset = 1f;
    public float detectionCooldown = 2f;
    private float lastDetectionTime = 0f;
    public int centerWidth = 100; // Width of the central detection area
    public int centerHeight = 100; // Height of the central detection area

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

        if (webcamTexture.isPlaying && webcamTexture.didUpdateThisFrame && Time.time - lastDetectionTime >= detectionCooldown)
        {
            int centerX = webcamTexture.width / 2;
            int centerY = webcamTexture.height / 2;
            int startX = Mathf.Max(centerX - centerWidth / 2, 0);
            int startY = Mathf.Max(centerY - centerHeight / 2, 0);
            int endX = Mathf.Min(centerX + centerWidth / 2, webcamTexture.width);
            int endY = Mathf.Min(centerY + centerHeight / 2, webcamTexture.height);

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    Color pixel = webcamTexture.GetPixel(x, y);
                    float redDifference = ColorDifference(pixel, targetColorRed);
                    float greenDifference = ColorDifference(pixel, targetColorGreen);

                    if (redDifference < colorThreshold)
                    {
                        Debug.Log("Red color detected!");
                        SpawnPowerUp(bombPowerUpPrefab);
                        lastDetectionTime = Time.time;
                        return;
                    }
                    else if (greenDifference < colorThreshold)
                    {
                        Debug.Log("Green color detected!");
                        SpawnPowerUp(greenPowerUpPrefab);
                        lastDetectionTime = Time.time;
                        return;
                    }
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
            Vector3 spawnPosition = spawnObject.transform.position;
            spawnPosition += spawnObject.transform.forward * zOffset;
            spawnPosition += spawnObject.transform.right * xOffset;
            spawnPosition += spawnObject.transform.up * yOffset;

            GameObject spawnedPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
            Destroy(spawnedPowerUp, 5f);
        }
        else
        {
            Debug.LogWarning("Spawn object or power-up prefab not assigned.");
        }
    }
}
