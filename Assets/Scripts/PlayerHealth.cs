using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    private float currentHealth;

    public GameObject playerCamera;
    private PlayerMovement playerMovement;
    private PlayerCameraControl cameraControl;
    private Player player;
    private bool isDead = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        cameraControl = GetComponent<PlayerCameraControl>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isDead) return;
        if (Input.GetKeyDown(KeyCode.R)) LoadManager.loadLevel(SceneManager.GetActiveScene().name);
    }

    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        checkHealth();
    }

    public void heal(float amount)
    {
        currentHealth = currentHealth + amount > maxHealth ? maxHealth : currentHealth + amount;
    }


    private void checkHealth()
    {
        if (isDead) return;
        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            playerMovement.enabled = false;
            player.enabled = false;
            cameraControl.enabled = false;

            Vector3 cameraPos = playerCamera.transform.localPosition;
            playerCamera.transform.localPosition = new Vector3(cameraPos.x, cameraPos.y - 1.0f, cameraPos.y);
        }
    }

    public float getMaxHealth() { return maxHealth; }
    public float getCurrentHealth() { return currentHealth; }
}
