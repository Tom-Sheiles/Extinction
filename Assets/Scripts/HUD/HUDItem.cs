using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDItem : MonoBehaviour
{
    // The name to use for the HUD item
    public string itemName = "";

    // The image of the item
    public Image itemImage;

    // The keybind that the item has set to it
    public Text itemKeybinding;
}
