using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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


    // Start is called before the first frame update
    void Start()
    {
        // Set the weapon that the player starts with.
        if (selectedItem == null)
        {
            HideItems();
            SetSelectedItem(secondaryWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // See if the player has changed items.
        CheckForItemChange();
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
