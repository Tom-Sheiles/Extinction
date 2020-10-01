using UnityEngine;

public class Bandage : MonoBehaviour, IPlayerItem
{
    // The name of the item
    public string itemName = "Bandage";

    // Maximum number of bandages that the player can hold
    public int maximumNumberOfBandages = 5;

    // Number of bandages that the player current has
    public int numberOfBandages = 2;

    // The time that it takes to use a bandage
    public float timeToBandage = 4.0f;

    public int bandageHealAmount = 25;

    // The sound to play when using a bandage
    public AudioClip useBandageSound;

    // Expends a bandage
    public void UseBandage()
    {
       numberOfBandages--;
    }

    public bool HasBandages()
    {
        return numberOfBandages > 0;
    }

    #region IPlayerItem Interface Methods
    public int GetCurrentItemAmount()
    {
        return 1;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public AudioClip GetItemSound()
    {
        return useBandageSound;
    }

    public int GetReserveItemAmount()
    {
        return numberOfBandages;
    }

    public bool HasSound()
    {
        return useBandageSound != null;
    }

    #endregion
}
