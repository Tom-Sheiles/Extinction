using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class that defines the behaviour for each state
public abstract class State
{
    // Reference to the object that owns the state
    protected GameObject stateContext;
    protected State nextState;

    // Is run once when the state is switched to
    public virtual void onStateEnter(GameObject context) { stateContext = context; }

    // Run once per frame while the state is active
    public virtual void onStateTick() { return; }

    // Returns the next state the machine should switch to
    public virtual State onStateExit() { return nextState; }

    // Checks the conditions to switch the machines state.
    // Returns true if the state should switch on the next frame
    public virtual bool checkStateSwitch() { return false; }
}