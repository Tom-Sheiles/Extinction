using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    // All information reguarding player health
    public PlayerHealth health;

    // Primary weapon
    public GameObject primaryWeapon;

    // Secondary weapon
    public GameObject secondaryWeapon;

    // Players healing supplies
    public GameObject bandages;

    // Explosives to destroy lab
    public GameObject explosives;

    // The currently select item
    public GameObject selectedItem;

    // Reference for any interactable objects that the player encounters
    public GameObject interactableObject;

    // Position where weapon fire originate from
    public Camera playerPOV;

    // Additive time since the last frame completion 
    private float timeElapsed = 0.0f;

    // Flag indicating if reloading is happening
    private bool isReloading = false;

    // Flag indicating if the player is using a banadge
    private bool isBandaging = false;

    // Start is called before the first frame update
    void Start()
    {        
        // Hide all items other than selected.
        HideItems();

        // Get the health script
        health = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Set player POV camera ot the camera that is attached to the FirstPersonCharacter.
        playerPOV = GetComponentInChildren<Camera>();

        if (!isReloading || !isBandaging)
        {
            // See if the player has changed items.
            CheckForItemChange();

            // See if the player has hit the use item button.
            CheckForItemUsage();

            // See if the player has hit the reload button.
            CheckForReload();
            
            // See if the player has interacted with something
            CheckForInteraction();
        }
    }

    // When an interactable item is found then set it.
    public void InteractableFound(GameObject interactableItem)
    {
        interactableObject = interactableItem;
    }

    // Checks to see if a weapon is selected.
    private bool HasWeaponSelected()
    {
        return selectedItem == primaryWeapon || selectedItem == secondaryWeapon;
    }

    // Checks if the player has the bandages selected.
    private bool HasBandagesSelected()
    {
        return selectedItem == bandages;
    }

    // Checks to see if keys 1-4 have been pressed to trigger item change.
    private void CheckForItemChange()
    {
        GameObject item = null;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            item = primaryWeapon;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            item = secondaryWeapon;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            item = bandages;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            item = explosives;
        }

        if (item)
        {
            SetSelectedItem(item);
        }
    }

    // Checks if the fire button (left click) has been pressed or is being held.
    private void CheckForItemUsage()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButton(0))
        {
            if (HasWeaponSelected())
            {
                Weapon weapon = selectedItem.GetComponent<Weapon>();
                if (timeElapsed >= weapon.fireRate)
                {
                    FireWeapon(weapon);
                    timeElapsed = 0.0f;
                }
            }
            else if (HasBandagesSelected())
            {
                Bandage bandage = selectedItem.GetComponent<Bandage>();
                StartCoroutine(UseBandage(bandage));
            }
        }
    }

    // Checks if the player has pressed the reload key (R)
    private void CheckForReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && HasWeaponSelected())
        {
            StartCoroutine(ReloadWeapon(selectedItem.GetComponent<Weapon>()));
        }
    }

    // Check if the player has interacted with something.
    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject)
        {
            if (interactableObject.GetComponent<Loot>())
            {
                PickedUpLoot(interactableObject.GetComponent<Loot>());
            }
            interactableObject.SendMessage("Interact");
            interactableObject = null;
        }
    }

    // Handles the firing of the selected weapon
    private void FireWeapon(Weapon weapon)
    {
        // Store information about the target that was hit
        RaycastHit hit;

        // Play the sound of the weapon firing
        PlaySound(weapon.IsLoaded() ? weapon.fireSound : weapon.noAmmunitionSound);

        if (weapon.IsLoaded())
        {         
            // Expend a bullet when the weapon is fired
            weapon.ammunitionInMagazine--;

            // Check if anything was hit within the weapons range
            if (Physics.Raycast(playerPOV.transform.position, playerPOV.transform.forward, out hit, weapon.weaponRange))
            {
                switch (hit.transform.tag) {

                    case "LightBulb":
                        hit.transform.GetComponent<AudioSource>().Play();
                        hit.transform.GetComponentInChildren<Light>().enabled = false;
                        break;

                    case "EnemyHead":
                        EnemyHealth enemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
                        enemyHealth.takeDamage(enemyHealth.getMaxHealth());
                        break;

                    case "EnemyBody":
                        hit.transform.GetComponent<EnemyHealth>().takeDamage(weapon.weaponDamage);
                        break;

                    default:
                        Instantiate(weapon.bulletHole, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal));
                        break;
                }
            }
        }
        else
        {
            StartCoroutine(ReloadWeapon(weapon));
        }
    }

    // Tries to reload the currently selected weapon.
    private IEnumerator ReloadWeapon(Weapon weapon)
    {
        if (weapon.CanReload())
        {
            // If already reloading then discontinue
            if (isReloading)
            {
                yield break;
            }

            // Weapon is now reloading, wait for the reload time and then reload.
            isReloading = true;
            PlaySound(weapon.reloadSound);
            yield return new WaitForSeconds(weapon.reloadTime);
            weapon.Reload();
            isReloading = false;            
        }
        else
        {
            // Notify player that they have no ammunition left (text in the middle of the screen or something).
        }
    }

    // Uses a bandage if the player chooses to do so
    private IEnumerator UseBandage(Bandage bandage)
    {
        if (bandage.HasBandages() && health.getCurrentHealth() < health.getMaxHealth())
        {
            // Break if player is already bandaging
            if (isBandaging)
            {
                yield break;
            }

            isBandaging = true;
            PlaySound(bandage.useBandageSound);
            yield return new WaitForSeconds(bandage.timeToBandage);
            bandage.UseBandage();
            SendMessage("heal", bandage.bandageHealAmount);
            isBandaging = false;
        }
        else
        {
            // No bandages left.
        }
    }

    // Plays a sound associated with a weapon
    private void PlaySound(AudioClip soundClip)
    {
        AudioSource audio = selectedItem.GetComponent<AudioSource>();
        audio.PlayOneShot(soundClip);
    }

    // Hides all items other than secondary weapon.
    private void HideItems()
    {
        GameObject[] items = { secondaryWeapon, bandages, explosives };

        // Loop over each item to hide.
        foreach(var item in items)
        {
            if (item)
            {
                SetItemVisibility(item, false);
            }
        }
    }

    // Loops over all the MeshRenderers of the item and hides them.
    private void SetItemVisibility(GameObject item, bool visibility)
    {
        // Get each MeshRenderer in the item and hide them.
        foreach (var itemMeshRenderer in item.GetComponentsInChildren<MeshRenderer>())
        {
            itemMeshRenderer.enabled = visibility;
        }
    }

    // Sets the selected item.
    private void SetSelectedItem(GameObject item)
    {
        if (selectedItem)
        {
            SetItemVisibility(selectedItem, false);
            SetItemVisibility(item, true);
        }

        selectedItem = item;
    }

    // Handles the picking up of loot.
    private void PickedUpLoot(Loot loot)
    {
        GameObject lootForItem = null;

        switch(loot.lootType)
        {
            case LootType.AR:
                lootForItem = primaryWeapon;
                break;

            case LootType.PISTOL:
                lootForItem = secondaryWeapon;
                break;

            case LootType.BANDAGE:
                lootForItem = bandages;
                break;

            default:
                break;
        }

        if (lootForItem)
        {
            lootForItem.GetComponent<IPlayerItem>().Add(loot.amount);
        }
    }
}
