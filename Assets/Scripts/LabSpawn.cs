using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabSpawn : MonoBehaviour
{
    private bool spawned = false;

    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            var message = "";
            if (player.HasDestroyedLab())
            {
                player.SendMessage("InteractableFound", gameObject);
                message = "Press 'E' to leave the lab";
            } 
            else
            {
                message = "The laboratory has not been destroyed!";
            }

            GameObject.FindGameObjectWithTag("HUD").SendMessage("DisplayMessage", message);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") && !spawned)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            spawned = true;
            player.transform.position = transform.position;
        }
    }

    public void Interact()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    private IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetSceneByName("Level01_GriffithUniversity"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
