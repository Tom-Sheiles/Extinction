using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Player health bar
    public Image healthBar;
    public Text healthText;

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

    public Color lowHealthColor;
    public HealthFlash healthFlash;
    private Color defaultHealthColor;
    PlayerHealth playerHealth;
    float healthBarInitialWidth;
    float healthBarCurrentWidth;
    float lastHealth;

    // Any message that the player receives.
    public Text gameMessage;

    private float elapsedMessageTime = 0.0f;

    private float messageTimer = 3.0f;

    // Detonation
    public Text detonationTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that there is a player
        if (player)
        {
            UpdateActiveItem(player.selectedItem.GetComponent<IPlayerItem>().ToString());
            UpdateActiveItemValues(player.selectedItem.GetComponent<IPlayerItem>());
            SetCrosshairSize();

            playerHealth = player.GetComponent<PlayerHealth>();
            healthBarInitialWidth = healthBar.rectTransform.rect.width;
            defaultHealthColor = healthBar.color;
            lastHealth = 100;
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
        elapsedMessageTime += Time.deltaTime;

        // Lower the update rate a bit.
        if (timeSinceLastUpdate >= updateRate)
        {
            timeSinceLastUpdate = 0.0f;
            UpdateActiveItem(player.selectedItem.GetComponent<IPlayerItem>().GetItemName());
            UpdateActiveItemValues(player.selectedItem.GetComponent<IPlayerItem>());

            float currentHealth = playerHealth.getCurrentHealth();
            float maxHealth = playerHealth.getMaxHealth();

            healthText.text = currentHealth.ToString();
            float healthPercent = currentHealth / maxHealth;

            healthBarCurrentWidth = healthBarInitialWidth * healthPercent;
            healthBar.rectTransform.sizeDelta = new Vector2(healthBarCurrentWidth, healthBar.rectTransform.rect.height);

            // If damage has been take since last update
            if (currentHealth < lastHealth)
            {
                healthFlash.fadeIn();
            }

            if (currentHealth <= (maxHealth / 2))
            {
                healthBar.color = lowHealthColor;
            }
            else
            {
                healthBar.color = defaultHealthColor;
            }

            lastHealth = currentHealth;

        }
    }


    // Updates the border around the items to indicate which one is selected.
    public void UpdateActiveItem(string selectedItem)
    {
        foreach (HUDItem item in playerItems)
        {
            // Get the image component of the HUD items parent.
            Image parentImage = item.GetComponentInParent<Image>();

            // If the item name is equal to the selected item then change the sprite around the item.
            Debug.Log($"item name: {item.itemName}, selectedItem: {selectedItem}");
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

    // Sets the width and height of the crosshair.
    public void SetCrosshairSize()
    {
        RectTransform crosshairDimensions = crosshair.GetComponent<RectTransform>();
        crosshairDimensions.sizeDelta = new Vector2(crosshairWidth, crosshairHeight);
    }

    // Displays a message to the player
    public void DisplayMessage(string message)
    {
        if (gameMessage.text != message || elapsedMessageTime >= messageTimer) 
        {
            elapsedMessageTime = 0.0f;
            gameMessage.text = message;
            gameMessage.color = Color.Lerp(new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), .25f);
            StartCoroutine("RemoveMessage");
        }
    }

    // Updates the detontation timer
    public void UpdateDetonationTimer(int timeTilDetonation)
    {
        detonationTimer.text = timeTilDetonation > 0 ? $"Bomb Detonation In: {timeTilDetonation}" : "";
    }

    // Removes a displayed message after 3 seconds.
    private IEnumerator RemoveMessage()
    {
        gameMessage.color = Color.Lerp(new Color(255, 255, 255, 0), new Color(255, 255, 255, 1), 3.0f);
        yield return new WaitForSeconds(3);
        gameMessage.text = "";
    }
}
