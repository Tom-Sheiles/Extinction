using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infected : MonoBehaviour
{
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
    public bool drawVisionRanges = true;
    public Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDrawGizmos()
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


        if (drawVisionRanges)
        {
            Gizmos.DrawWireSphere(transform.position, maxVisionDistance);
            Gizmos.color = Color.red;
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
}
