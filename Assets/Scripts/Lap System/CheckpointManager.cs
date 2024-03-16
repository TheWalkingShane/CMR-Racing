using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static int currentCheckpoint = 0;
    public int checkpointNumber;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && checkpointNumber == currentCheckpoint)
        {
            
            currentCheckpoint++;
            Debug.Log("Current Checkpoint" + currentCheckpoint);
        }
    }
}