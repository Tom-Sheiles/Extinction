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
        checkHealth();
    }

    private void checkHealth()
    {
        if(currentHealth <= 0)
        {
            Debug.Log(gameObject.name + " Died");
            stateMachine.setCurrentState(new Dead());
        }
    }

    public float getMaxHealth() { return maxHealth; }
    public float getCurrentHealth() { return currentHealth; }
}
