using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera staticCamera;  // The static camera in the hierarchy
    private Camera carCamera;    // The dynamically found camera on the car

    // Call this method when the button is clicked
    public void SwitchCamera()
    {
        // Ensure the car camera is found, try to find if null
        if (carCamera == null)
        {
            FindCarCamera();
        }

        // Toggle active state between cameras
        if (carCamera != null && carCamera.gameObject.activeInHierarchy)
        {
            // If car camera is active, switch to static camera
            carCamera.gameObject.SetActive(false);
            staticCamera.gameObject.SetActive(true);
        }
        else if (carCamera != null)
        {
            // If static camera is active, switch to car camera
            staticCamera.gameObject.SetActive(false);
            carCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Car camera not found!");
        }
    }

    private void FindCarCamera()
    {
        // Assuming the car has a tag "Car" and the main camera is a child of the car
        GameObject car = GameObject.FindGameObjectWithTag("Car");
        if (car != null)
        {
            carCamera = car.GetComponentInChildren<Camera>();
        }
    }
}