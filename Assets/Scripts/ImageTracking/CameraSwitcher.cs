using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera portraitCamera;
    public Camera landscapeCamera;

    public GameObject portraitUI;
    public GameObject landscapeUI;

    // Call this method when the button is clicked
    public void SwitchCamera()
    {
        if (portraitCamera.gameObject.activeInHierarchy)
        {
            // Switch to landscape camera
            portraitCamera.gameObject.SetActive(false);
            landscapeCamera.gameObject.SetActive(true);

            // Switch UI
            portraitUI.SetActive(false);
            landscapeUI.SetActive(true);

            // Change screen orientation
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            // Switch to portrait camera
            landscapeCamera.gameObject.SetActive(false);
            portraitCamera.gameObject.SetActive(true);

            // Switch UI
            landscapeUI.SetActive(false);
            portraitUI.SetActive(true);

            // Change screen orientation
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}

