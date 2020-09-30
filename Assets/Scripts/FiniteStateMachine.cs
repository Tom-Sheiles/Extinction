using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    public float AITickRate = 0.15f;
    private State currentState;
    public bool machinePaused = false;
    private bool initalized = false;

    [HideInInspector][SerializeField]public int stateIndex;

    private float timeSinceLastUpdate = 0f;

    // Finds reference to all subclasses of MachineType and initalizes the state machine selected in the inspector.
    public void StartStateMachine()
    {
        List<MachineType> types = FindSubtypes.FindGenericSubtypes<MachineType>();

        // The First state in the selected type is set as the initial
        currentState = types[stateIndex].possibleStates[0];
        currentState.onStateEnter(gameObject);
        initalized = true;
    }

    public string getCurrentState()
    {
        if (currentState != null)
            return currentState.ToString();
        else
            return "None";
    }

    public void setCurrentState(State state)
    {
        currentState.onStateExit();
        currentState = state;
        currentState.onStateEnter(gameObject);
    }


    private void Update()
    {
        if (!initalized || machinePaused) return;

        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= AITickRate)
        {
            AITickRate = 0f;
            if (!currentState.checkStateSwitch())
            {
                currentState.onStateTick();
            }
            else
            {
                State newState = currentState.onStateExit();
                currentState = newState;
                currentState.onStateEnter(gameObject);
            }
        }
    }
}
