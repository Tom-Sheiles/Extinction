using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads a level with the given levelName
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
