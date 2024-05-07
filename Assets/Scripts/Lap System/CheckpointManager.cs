using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static int currentCheckpoint = 0;
    public static int totalCheckpoints = 5;
    public static int currentLap = 0; // Now starting from 0 for the preparation lap
    public static int totalLaps = 3;

    public delegate void LapChangeAction(int currentLap);
    public static event LapChangeAction OnLapChange;

    public static void AdvanceCheckpoint()
    {
        currentCheckpoint++;
        if (currentCheckpoint >= totalCheckpoints)
        {
            currentCheckpoint = 0; // Reset for the next lap
            AdvanceLap();
        }
    }

    private static void AdvanceLap()
    {
        currentLap++;
        if (currentLap <= totalLaps)
        {
            OnLapChange?.Invoke(currentLap); // Notify subscribers about the lap change
        }
        else
        {
            Debug.Log("Race Finished!");
            // Handle race completion logic here
        }
    }
}
