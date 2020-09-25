using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPlayerItem
{
    public string weaponType = "";

    public int maximumStackSize = 12;

    public int currentStackSize = 12;

    public float fireRate = 0.5f;


    public float GetFireRate()
    {
        return fireRate;
    }

    public int GetMaximumStackSize()
    {
        return maximumStackSize;
    }

    public int GetCurrentStackSize()
    {
        return currentStackSize;
    }

    public override string ToString()
    {
        return weaponType;
    }
}
