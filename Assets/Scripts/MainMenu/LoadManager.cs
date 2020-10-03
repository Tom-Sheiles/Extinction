using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadManager
{
    public static void loadLevel(string levelName)
    {
        if (SceneManager.GetActiveScene().name != "Loading")
            SceneManager.LoadScene("Loading");
        else
            SceneManager.LoadSceneAsync(levelName);
    }
}
