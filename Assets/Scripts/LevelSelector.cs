using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Method to load a specific level based on its name
    public void LoadLevel(string levelName)
    {
        // Load the scene with the given name
        SceneManager.LoadScene(levelName);
    }
}