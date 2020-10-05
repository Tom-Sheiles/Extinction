using UnityEngine;
using UnityEngine.SceneManagement;

public class Extraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            string message = "";
            if (player.CanBeExtracted())
            {
                player.SendMessage("InteractableFound", gameObject);
                message = "Press 'E' to extract.";
            } 
            else
            {
                message = "You cannot be extracted. You must complete all of the objectives.";
            }

            GameObject.FindGameObjectWithTag("HUD").SendMessage("DisplayMessage", message);
        }
    }

    public void Interact()
    {
        // Load the end stats page.
        SceneManager.LoadScene("LevelComplete");
    }
}
