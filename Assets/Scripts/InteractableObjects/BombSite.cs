using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BombSite : MonoBehaviour
{
    // Range in which the player can plant the explosive
    private float interactableRange = 4.0f;

    // Place to plant the explosive
    private GameObject explosive;

    // The players Heads Up Display
    private GameObject HUD;

    // Flag to indicate if the explosive is in a tick
    private bool isInTick = false;

    // Flag to indicate if the explosive is detonating
    private bool explosiveIsDetonating = false;


    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
    }

    // Update is called once per frame
    void Update()
    {
        if (!explosive)
        {
            CheckForPlayerInRange();
        }
        else
        {
            // Explosives have been planted so start counting down.
            var explosive = this.explosive.GetComponent<Explosive>();
            if (!isInTick)
            {
                if (explosive.TimeTilDetonation() > 0)
                {
                    isInTick = true;
                    StartCoroutine(Countdown(explosive));
                } else
                {
                    if (!explosiveIsDetonating)
                    {
                        explosiveIsDetonating = true;
                        explosive.Detonate();
                    }
                }
            }
        }
    }

    // Instantiates an explosives gameobject within the bombsite
    public void PlantExplosive(GameObject explosive)
    {        
        this.explosive = Instantiate(explosive, transform);
    }

    // Updates the explosives time until detonation each second
    private IEnumerator Countdown(Explosive explosive)
    {
        yield return new WaitForSeconds(1);
        explosive.Countdown();
        HUD.SendMessage("UpdateDetonationTimer", explosive.TimeTilDetonation());
        isInTick = false;
    }

    // Checks if the player is in range to plant the explosive
    private void CheckForPlayerInRange()
    {
        foreach (var objectInRange in Physics.OverlapSphere(transform.position, interactableRange))
        {
            if (objectInRange.CompareTag("Player"))
            {
                // Tell the player that they have entered the bombsite
                objectInRange.SendMessage("InteractableFound", gameObject);

                // If player is in range check that they have explosives equipped.
                var playerItem = HUD.GetComponent<HUD>().player.selectedItem.GetComponent<IPlayerItem>();
                HUD.SendMessage("DisplayMessage", playerItem.GetItemName().CompareTo("explosive") == 0 ? "Press 'Left Mouse' to plant explosives" : "Select explosives (4) to plant");                
            }
        }
    }
}
