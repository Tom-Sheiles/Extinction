using System.Security.Policy;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // How far away the player can be to interact with the item.
    private float interactableRange = 2.0f;

    // The name of the loot item
    public string lootName = "";

    // Item that the loot is for
    public LootType lootType;

    // The amount of the item that is given
    public int amount;

    // Flag to see if the loot item has been picked up
    public bool isActive = true;

    // Where messages are displayed
    public GameObject HUD;


    // Start is called before the first frame update
    void Start()
    {
        // Get the HUD
        HUD = GameObject.FindGameObjectWithTag("HUD");

        if (isActive)
        {
            CheckForNearbyPlayer();
            Animate();
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (isActive)
       {
            Animate();
       }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, 2);
    }

    // Destroy the item when picked up.
    public void Interact()
    {
        Destroy(gameObject);
    }

    private void CheckForNearbyPlayer()
    {
        var objectsInRange = Physics.OverlapSphere(transform.position, interactableRange);
        foreach (var objectInRange in objectsInRange)
        {
            if (objectInRange.CompareTag("Player"))
            {
                // Pass this object to the player to be able to interact with it.
                objectInRange.SendMessage("InteractableFound", gameObject);

                // Tell the player that they can open the box.
                HUD.SendMessage("DisplayMessage", $"Press 'E' to pickup {lootName}");
            }
        }
    }

    // Play the loot animation
    private void Animate()
    {
        GetComponent<Animation>().Play();
    }
}
