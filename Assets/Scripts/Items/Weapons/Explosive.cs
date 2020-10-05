using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour, IPlayerItem
{
    // Name of the item
    public string itemName = "explosive";

    // Number of explosives the player has
    public int numberOfExplosives = 1;

    // Number of explosives the player has in reserve
    public int explosivesInReserve = 0;

    // Number of explosives the player can hold at once
    public int maximumNumberOfExplosives = 1;

    // Area in which you will die if you are inside when the bomb goes off.
    public float explosiveDeathRadius = 100.0f;

    // Area in which you will take damage if you are inside when the bomb goes off
    public float explosiveDamageRadius = 200.0f;

    // Time that it takes for the explosive to detonate
    private int detonationTime = 20;

    // Time left until detonation
    private int timeTilDetonation;

    // Time it takes to plant the bomb
    public int timeToPlant = 3;

    // Sound to play when planting the bomb
    public AudioClip plantSound;

    // Early countdown bomb tick sound
    public AudioClip bombTick;

    // Quick countdown bomb tick sound
    public AudioClip bombTickQuick;

    // Sound that plays when the bomb detonates
    public AudioClip detonationSound;

    // Start is called before the first frame update
    void Start()
    {
        timeTilDetonation = detonationTime;
    }


    // Checks if the player has any explosives
    public bool HasExplosive()
    {
        return numberOfExplosives > 0;
    }

    // Gets the time until the explosive detonates
    public int TimeTilDetonation()
    {
        return timeTilDetonation;
    }

    // Counts down the time until detonation, playing tick sounds
    public void Countdown()
    {
        AudioClip clip = null;
        if (timeTilDetonation >= 10)
        {
            clip = bombTick;
        }
        if (timeTilDetonation < 10 && timeTilDetonation > 1)
        {
            clip = bombTickQuick;
        }        

        timeTilDetonation--;
        if (clip)
        {
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }

    // Detonates the explosive
    public void Detonate()
    {
        GetComponent<AudioSource>().PlayOneShot(detonationSound);
        StartCoroutine(CheckIfPlayerNearby());

        // Update the score when the explosive is detonated
        GameObject.FindGameObjectWithTag("Score").SendMessage("ExplosiveDetonated");

        Destroy(gameObject, detonationSound.length);
    }

    // Plants the explosive
    public void Plant()
    {
        GetComponent<AudioSource>().PlayOneShot(plantSound);
        GetComponent<Animation>().Play();
        StartCoroutine(WaitForPlantToUseExplosive());
    }


    #region IPlayerItem Interface Methods

    public void Add(int amount)
    {
        throw new System.NotImplementedException();
    }

    public int GetCurrentItemAmount()
    {
        return numberOfExplosives;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public AudioClip GetItemSound()
    {
        throw new System.NotImplementedException();
    }

    public int GetReserveItemAmount()
    {
        return explosivesInReserve;
    }

    public bool HasSound()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    // Waits for the length of the bomb plant until removing the explosive.
    private IEnumerator WaitForPlantToUseExplosive()
    {
        yield return new WaitForSeconds(timeToPlant);
        numberOfExplosives--;

        // Update the score when the explosive is planted
        GameObject.FindGameObjectWithTag("Score").SendMessage("ExplosivePlanted");
    }

    // Checks if a player is nearby when the explosion goes off and deals damage based on the players range to the explosion
    private IEnumerator CheckIfPlayerNearby()
    {
        yield return new WaitForSeconds(2);
        var player = GameObject.FindGameObjectWithTag("Player");
        var distance = Vector3.Distance(transform.position, player.transform.position);
        var playerHealth = player.GetComponent<PlayerHealth>();
        var damage = 0;
        if (distance <= explosiveDeathRadius)
        {
            damage = (int)playerHealth.getMaxHealth();
        }
        else if (distance > explosiveDeathRadius && distance <= explosiveDamageRadius)
        {
            damage = (int)(playerHealth.getMaxHealth() / (distance * 5));
        }

        playerHealth.SendMessage("takeDamage", damage);
    }
}
