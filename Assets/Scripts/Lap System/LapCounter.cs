using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    public int totalLaps = 3;
    public int currentLap = 0;
    public TextMeshProUGUI lapCounterText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && CheckpointManager.currentCheckpoint == 4) // Assuming 4 checkpoints
        {
                                                                                Debug.Log("Lap completed!");
            currentLap++;
                                                                                Debug.Log("Current Lap" + currentLap);
            CheckpointManager.currentCheckpoint = 0; // Reset for the next lap
            
            // this writes to the counter to increase on the UI
            if (currentLap <= totalLaps)
            {
                lapCounterText.text = "Lap: " + currentLap + "/" + totalLaps;
            }

            if (currentLap <= totalLaps)
            {
                // enter a finish state then move to next level
            }
        }
    }
}   