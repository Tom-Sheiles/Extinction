using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Player health bar
    public Image healthBar;

    // Original size of the health bar
    public float healthBarWidth;

    // Player health text
    public Text health;

    // Player crosshair
    public Image crosshair;

    // Crosshair width
    public float crosshairWidth = 30.0f;

    // Crosshair height
    public float crosshairHeight = 30.0f;

    // Items that the player has
    public HUDItem[] playerItems;

    // The player object
    public Player player;

    // Display the maximum stack / clip size of the selected item
    public Text maximumStackSize;

    // Display the current stack / clip size of the selected item
    public Text currentStackSize;

    // Sprite to use if an item is selected
    public Sprite selectedItemSprite;

    // Sprite to use if an item is not selected.
    public Sprite unselectedItemSprite;

    // The time that has occured since the last HUD update.
    private float timeSinceLastUpdate = 0.0f;

    // How often the HUD should update.
    private float updateRate = 0.1f;

    // Keeps track of the players health the last time that it was updated
    private int playerHealthLastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that there is a player
        if (player)
        {
            
            UpdateActiveItem(player.selectedItem.GetComponent<IPlayerItem>().ToString());
            UpdateActiveItemValues(player.selectedItem.GetComponent<IPlayerItem>());
            SetCrosshairSize();

            healthBarWidth = healthBar.rectTransform.rect.width;
        } 
        else
        {
            print("There is no player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        // Lower the update rate a bit.
        if (timeSinceLastUpdate >= updateRate)
        {            
            timeSinceLastUpdate = 0.0f;          
            UpdateActiveItem(player.selectedItem.GetComponent<IPlayerItem>().GetItemName());
            UpdateActiveItemValues(player.selectedItem.GetComponent<IPlayerItem>());
            
            // Only update if the players health has changed
            if (PlayerHealthHasChanged())
            {
                UpdatePlayerHealth();
            }
        }        
    }

    // Checks if the players health has changed since the last update
    private bool PlayerHealthHasChanged()
    {
        return player.playerHealth != playerHealthLastUpdate;
    }

    // Updates the border around the items to indicate which one is selected.
    public void UpdateActiveItem(string selectedItem)
    {
        foreach(HUDItem item in playerItems)
        {
            // Get the image component of the HUD items parent.
            Image parentImage = item.GetComponentInParent<Image>();

            // If the item name is equal to the selected item then change the sprite around the item.
            if (item.itemName == selectedItem)
            {
                parentImage.sprite = selectedItemSprite;
            }
            else
            {
                parentImage.sprite = unselectedItemSprite;
            }
        }
    }

    // Updates the current and maximum stack / clip information.
    public void UpdateActiveItemValues(IPlayerItem item)
    {
        maximumStackSize.text = item.GetReserveItemAmount().ToString();
        currentStackSize.text = item.GetCurrentItemAmount().ToString();
    }

    // Sets the dimensions of the crosshair.
    public void SetCrosshairSize()
    {
        RectTransform crosshairDimensions = crosshair.GetComponent<RectTransform>();
        crosshairDimensions.sizeDelta = new Vector2(crosshairWidth, crosshairHeight);
    }

    // Updates the player health information
    private void UpdatePlayerHealth()
    {
        playerHealthLastUpdate = player.playerHealth;

        health.text = player.playerHealth.ToString();
        RectTransform healthBarDimensions = healthBar.GetComponent<RectTransform>();
        healthBarDimensions.sizeDelta = new Vector2(((float)player.playerHealth / player.playerMaximumHealth) * healthBarWidth, healthBarDimensions.sizeDelta.y);

        // Change color based on health thresholds
        Image healthBarImage = healthBar.GetComponent<Image>();
        healthBarImage.color = player.playerHealth > 50 ? Color.white : Color.red;
    }
}
