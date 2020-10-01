using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infected : MonoBehaviour, IMelee
{
    [Header("Movement Variables")]
    public Transform[] waypoints;
    public float waypointStopDistance = 0.1f;
    public float walkingSpeed = 1f;
    public float chaseSpeed = 3.5f;
    public float visionAngle = 45f;
    public float crouchingVisionmultiplier = 0.5f;
    public float walkingVisionRange = 10f;
    public float RunningVisionRange = 20f;
    public float maxVisionDistance = 10f;
    public float distanceIncreaseRatio = 5f;
    public float noticeTime = 5f;

    [Header("Attack Variables")]
    public float attackDamge = 10f;
    public float attackSpeed = 1.5f;
    public float attackRange = 3.0f;

    [Range(0,100)]
    public float hitPercent = 1.0f;

    [Header("UI")]
    public GameObject visionIndicator;
    public Color alertColor;
    public Color spottedColor;
    [HideInInspector] public Image visionImage;
    [HideInInspector] public Transform playerTransform;

    [Header("Misc")]
    public bool drawVisionRanges = true;
    public bool drawPath = true;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        visionImage = visionIndicator.GetComponentInChildren<Image>();

        GetComponent<FiniteStateMachine>().StartStateMachine();
    }

    private void OnDrawGizmos()
    {
        if (drawPath)
        {
            Gizmos.color = Color.yellow;

            if (waypoints.Length > 1)
            {
                for (int i = 0; i < waypoints.Length - 1; i++)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
            }
        }

        if (drawVisionRanges)
        {
            Gizmos.DrawWireSphere(transform.position, maxVisionDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Vector3 pos = transform.position;
            pos.y += 1;

            Quaternion left = Quaternion.AngleAxis(-visionAngle / 2, Vector3.up);
            Quaternion right = Quaternion.AngleAxis(visionAngle / 2, Vector3.up);
            Vector3 lVector = left * transform.forward;
            Vector3 RVector = right * transform.forward;

            Gizmos.DrawRay(pos, lVector * maxVisionDistance);
            Gizmos.DrawRay(pos, RVector * maxVisionDistance);

        }
    }

    public float getDamage()
    {
        return attackDamge;
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
