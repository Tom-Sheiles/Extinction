﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedAnimal : MonoBehaviour, IMelee
{
    public float walkSpeed = 2.5f;
    public float chaseSpeed = 4f;
    public Transform[] waypoints;
    public float waypointStopDistance = 0.1f;
    FiniteStateMachine stateMachine;

    public float attackDamage = 15f;
    public float attackSpeed = 0.5f;
    public float attackRange = 2.0f;

    [Range(0,100)]
    public float hitPercent = 0.75f;

    public float chaseDistance = 10f;
    public float noticeRange = 20f;
    public GameObject visionIndicator;

    [HideInInspector]public Transform playerTransform;
    public bool drawGizmos = true;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.StartStateMachine();
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, noticeRange);
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

    public float getDamage()
    {
        return attackDamage;
    }

    public float getAttackSpeed()
    {
        return attackSpeed;
    }

    public float getAttackRange()
    {
        return attackRange;
    }

    public float getChaseSpeed()
    {
        return chaseSpeed;
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
