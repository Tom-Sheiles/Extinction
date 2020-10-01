using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    public float visionRadius = 20f;
    FiniteStateMachine stateMachine;
    public Transform[] waypoints;

    private void Start()
    {
        stateMachine = GetComponent<FiniteStateMachine>();

        stateMachine.StartStateMachine();
    }
}
