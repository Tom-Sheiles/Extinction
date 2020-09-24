using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infected : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointStopDistance = 0.1f;
    public float walkingSpeed = 1f;
    public float visionAngle = 45f;
    public float crouchingVisionRange = 5f;
    public float walkingVisionRange = 10f;
    public float RunningVisionRange = 20f;
    public bool drawVisionRanges = true;
    public Transform playerTransform;

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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, walkingVisionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, crouchingVisionRange);
        }
    }
}
