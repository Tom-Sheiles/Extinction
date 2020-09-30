using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    private float currentHealth;
    private FiniteStateMachine stateMachine;

    private void Start()
    {
        stateMachine = GetComponent<FiniteStateMachine>();
        currentHealth = maxHealth;
    }

    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        //stateMachine.setCurrentState(new Chase());
        checkHealth();
    }

    private void checkHealth()
    {
        if(currentHealth <= 0)
        {
            stateMachine.setCurrentState(new Dead());
        }
    }

    public float getMaxHealth() { return maxHealth; }
    public float getCurrentHealth() { return currentHealth; }
}
