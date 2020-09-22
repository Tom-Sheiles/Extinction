using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private State currentState;
    public bool machinePaused = false;
    private bool initalized = false;

    [HideInInspector][SerializeField]public int stateIndex;

    private void Start()
    {
        StartStateMachine();
    }

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


    private void Update()
    {
        if (!initalized || machinePaused) return;

        if(!currentState.checkStateSwitch())
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
