using UnityEngine;

public class CollapsableBox : MonoBehaviour
{
    // The radius in which a player must be to .
    private float interactableRange = 2.0f;

    // Array of possible item spawns
    public GameObject[] possibleSpawns;

    // Flag indicating whether the object is interacable.
    public bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            var objectsInRange = Physics.OverlapSphere(transform.position, interactableRange);
            foreach (var obj in objectsInRange)
            {
                if (obj.CompareTag("Player"))
                {
                    // Pass this object to the player to be able to interact with it.
                    obj.SendMessage("InteractableFound", gameObject);

                    // Tell the player that they can open the box.
                    GameObject.FindGameObjectWithTag("HUD").SendMessage("DisplayMessage", "Press 'E' to open box");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
//        Gizmos.color = Color.red;
  //      Gizmos.DrawWireSphere(transform.position, interactableRange);
    }

    // Listens out for other objects calling this method.
    public void Interact()
    {
        Collapse();        
    }

    // Plays the collapse animation.
    private void Collapse()
    {
        GetComponent<Animation>().Play();
        isActive = false;
        int itemToSpawn = Random.Range(0, possibleSpawns.Length - 1);
        if (itemToSpawn > 0)
        {
            Instantiate(possibleSpawns[itemToSpawn], transform.position, transform.rotation);
        }

        // Update loot score
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().SendMessage("FoundLoot");
    }
}
