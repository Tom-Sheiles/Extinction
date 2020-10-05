using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabEntrance : MonoBehaviour
{
    private bool hasLeftLab = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("InteractableFound", gameObject);

            GameObject.FindGameObjectWithTag("HUD").SendMessage("DisplayMessage", "Press 'E' to enter the laboratory");
        }
    }

    public void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<Player>().HasDestroyedLab() && !hasLeftLab)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
            hasLeftLab = true;
            GameObject.FindGameObjectWithTag("Player").SendMessage("CompleteObjective", 4);
        }
    }

    public void Interact()
    {
        if(SceneManager.GetActiveScene().name == "Level01_GriffithUniversity")
        {
            StartCoroutine(LoadYourAsyncScene());
        }
        else
        {
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetSceneByName("Level01_GriffithUniversity"));
        }
    }

    private IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetSceneByName("Lab"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
