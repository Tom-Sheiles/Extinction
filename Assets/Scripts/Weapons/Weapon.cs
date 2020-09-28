using UnityEngine;

public class Weapon : MonoBehaviour, IPlayerItem
{
    // The type of weapon i.e. Primary or Secondary
    public string weaponType = "";

    // The maximum amount of ammunition that the player can carry for the weapon
    public int maximumAmmunition;

    // The maximum capacity of the weapon magazine
    public int magazineSize;

    // The current amount of ammunition in the magazine
    public int ammunitionInMagazine;

    // The amount of ammunition in reserve
    public int ammunitionInReserve;

    // the speed at which the weapon fires
    public float fireRate;

    // The ammount of damage that each bullet inflicts
    public float weaponDamage;

    // The range at which the weapon will hit enemies
    public float weaponRange;

    // The sound to play when the weapon is fired
    public AudioClip fireSound;

    // The sound to play when the weapon is out of ammunition
    public AudioClip noAmmunitionSound;

    // Object that will spawn if a bullet hits a solid object
    public GameObject bulletHole;

    // Check if the weapon is loaded
    public bool IsLoaded()
    {
        return ammunitionInMagazine > 0;
    }

    // See if their is enough ammunition to reload the weapon
    public bool CanReload()
    {
        return ammunitionInReserve > 0;
    }

    // Reloads the weapon
    public void Reload()
    {
        // Only reload if you have reserve ammunition and missing ammunition in the magazine
        if (ammunitionInReserve > 0 && ammunitionInMagazine < magazineSize)
        {
            var spaceInMagazine = magazineSize - ammunitionInMagazine;
            ammunitionInMagazine += ammunitionInReserve >= spaceInMagazine ? spaceInMagazine : ammunitionInReserve;
            ammunitionInReserve = ammunitionInReserve - spaceInMagazine >= 0 ? ammunitionInReserve - spaceInMagazine : 0;
        }
    }

    public int GetReserveItemAmount()
    {
        return ammunitionInReserve;
    }

    public int GetCurrentItemAmount()
    {
        return ammunitionInMagazine;
    }

    public AudioClip GetItemSound()
    {
        return fireSound;
    }

    public bool HasSound()
    {
        return fireSound != null;
    }
}
