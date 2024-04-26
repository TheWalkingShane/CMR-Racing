using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_Select"); // this loads the level select to pick the laps
    }
  
    public void QuitGame()
    {
        Application.Quit();
        UnityEngine.Debug.Log("Quitting"); 
    }

    
    
    
    //public GameObject optionsScreen;
    
    /*
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }
    */
}
