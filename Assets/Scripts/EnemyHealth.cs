using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    private float currentHealth;
    private FiniteStateMachine stateMachine;

    private bool headShot = false;

    private void Start()
    {
        stateMachine = GetComponent<FiniteStateMachine>();
        currentHealth = maxHealth;
    }

    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        headShot = amount >= maxHealth;
        //stateMachine.setCurrentState(new Chase());
        checkHealth();
    }

    private void checkHealth()
    {
        if(currentHealth <= 0)
        {
            stateMachine.setCurrentState(new Dead());
            GameObject.FindGameObjectWithTag("Score").SendMessage("KilledEnemy", headShot);
        }
    }

    public float getMaxHealth() { return maxHealth; }
    public float getCurrentHealth() { return currentHealth; }
}
