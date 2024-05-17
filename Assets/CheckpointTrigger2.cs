using UnityEngine;

public class CheckpointTrigger2 : MonoBehaviour
{
    public int checkpointNumber; // This public variable should appear in the Unity Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // Check if the player hits the checkpoints in the correct order
            if (checkpointNumber == CheckpointManager.currentCheckpoint)
            {
                CheckpointManager.AdvanceCheckpoint();
                Debug.Log("Checkpoint " + checkpointNumber + " hit.");
            }
            else
            {
                Debug.Log("Wrong checkpoint " + checkpointNumber + ". Expected: " + CheckpointManager.currentCheckpoint);
            }
        }
    }
}