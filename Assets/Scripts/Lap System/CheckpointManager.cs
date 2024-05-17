using UnityEngine;
using UnityEngine.SceneManagement; // Add this namespace for scene management

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
            EndGameAndGoToLevelSelector();
        }
    }

    private static void EndGameAndGoToLevelSelector()
    {
        // You may want to add some additional logic here for game cleanup or saving before loading the next scene
        SceneManager.LoadScene("Level_Select"); // Load the level selector scene
    }
}