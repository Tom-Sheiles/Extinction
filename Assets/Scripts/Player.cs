using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Primary weapon
    public GameObject primaryWeapon;

    // Secondary weapon
    public GameObject secondaryWeapon;

    // Players healing supplies
    public GameObject bandages;

    // Explosives to destroy lab
    public GameObject explosives;

    public GameObject selectedItem;

    // Position where weapon fire originate from
    public Camera playerPOV;

    // Additive time since the last frame completion 
    private float timeElapsed;

    // Flag indicating if reloading is happening
    private bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        // Set the weapon that the player starts with.
        if (selectedItem == null)
        {
            HideItems();
            SetSelectedItem(secondaryWeapon);
        }

        isReloading = false;
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Set player POV camera ot the camera that is attached to the FirstPersonCharacter.
        playerPOV = GetComponentInChildren<Camera>();

        if (!isReloading)
        {
            // See if the player has changed items.
            CheckForItemChange();

            // If the player has a firable item selected then check if the player is firing
            if (HasWeaponSelected())
            {
                CheckForWeaponFire();
                CheckForReload();
            }
        }
    }

    // Checks to see if a weapon is selected.
    private bool HasWeaponSelected()
    {
        return selectedItem == primaryWeapon || selectedItem == secondaryWeapon;
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
    private void CheckForWeaponFire()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetMouseButton(0))
        {
            Weapon weapon = selectedItem.GetComponent<Weapon>();
            if (timeElapsed >= weapon.fireRate)
            {
                FireWeapon(weapon);
                timeElapsed = 0.0f;
            }
        }
    }

    private void CheckForReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadWeapon(selectedItem.GetComponent<Weapon>()));
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

                // If we hit a light bulb then destroy it. (turn off light and play sound)
                if (hit.transform.name.ToLower().Contains("lightbulb"))
                {
                    hit.transform.GetComponent<AudioSource>().Play();
                    hit.transform.GetComponentInChildren<Light>().enabled = false;
                }
                else
                {       
                    // Applies bullet holes if walls or buildings are hit.
                    Instantiate(weapon.bulletHole, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal));            
                }

                // TODO: If an enemy is hit then see if it was a head or body shot and apply damage.
                // Headshot is a kill on a basic enemy so add enemy total health to weapon damage
                // Body shot just applies the weapons damage.
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

    // Plays a sound associated with a weapon
    private void PlaySound(AudioClip soundClip)
    {
        AudioSource audio = selectedItem.GetComponent<AudioSource>();
        audio.PlayOneShot(soundClip);
    }

    // Hides all items other than secondary weapon.
    private void HideItems()
    {
        GameObject[] items = { primaryWeapon, bandages, explosives };

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

    private void SetSelectedItem(GameObject item)
    {
        if (selectedItem)
        {
            SetItemVisibility(selectedItem, false);
            SetItemVisibility(item, true);
        }

        selectedItem = item;
    }
}
