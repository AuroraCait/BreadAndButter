using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

    // Play button sends to scene, determined in Unity via the On Click() tab - I set this to Game for the time being.
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quit functionality and debug.
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
