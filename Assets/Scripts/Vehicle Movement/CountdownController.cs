using System.Collections;
using TMPro; // Import TextMeshPro namespace
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public ARCarController carController;
    public TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component

    private void Start()
    {
        if (carController == null)
        {
            carController = FindObjectOfType<ARCarController>();
        }
        
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        carController.CanMove = false; // Disable movement

        // Start from 3 and count down to 0
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Update the countdown text
            countdownText.color = Color.white; // Optional: Change text color
            yield return new WaitForSeconds(1);
        }

        countdownText.text = "Go!";
        countdownText.color = Color.green; // Optional: Change text color to green when go
        yield return new WaitForSeconds(1); // Display "Go!" for 1 second

        countdownText.gameObject.SetActive(false); // Optionally hide the countdown text after it finishes

        carController.CanMove = true; // Enable movement
    }
}