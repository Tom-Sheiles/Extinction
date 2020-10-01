using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IMelee
{
    public float visionRange = 30f;

    public float attackRange;
    public float attackSpeed;
    public float chaseSpeed;
    public float attackDamage;

    [Range(0, 100)]
    public float hitPercent = 25.0f;

    private Transform playerTransform;
    private FiniteStateMachine stateMachine;

    private EnemyHealth health;
    private bool enraged = false;

    public bool drawGizmos = true;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = GetComponent<FiniteStateMachine>();
        health = GetComponent<EnemyHealth>();

        stateMachine.StartStateMachine();
    }


    private void Update()
    {
        if (enraged) return;

        if(health.getCurrentHealth() <= health.getMaxHealth()/2)
        {
            Debug.Log("Enraged");
            enraged = true;
            attackDamage *= 2;
            attackSpeed *= 2;

            chaseSpeed = 10f;
            hitPercent = 0;

            stateMachine.setCurrentState(new Chase());
        }
    }


    private void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRange);
        }
    }


    public float getAttackRange()
    {
        return attackRange;
    }

    public float getAttackSpeed()
    {
        return attackSpeed;
    }

    public float getChaseSpeed()
    {
        return chaseSpeed;
    }

    public float getDamage()
    {
        return attackDamage;
    }

    public Transform getTargetTransform()
    {
        return playerTransform;
    }

    public float hitChance()
    {
        return hitPercent;
    }
}
