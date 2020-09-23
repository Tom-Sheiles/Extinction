using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class that defines the behaviour for each state
public abstract class State
{
    // Reference to the object that owns the state
    protected GameObject stateContext;

    // Is run once when the state is switched to
    public virtual void onStateEnter(GameObject context) { stateContext = context; }

    // Run once per frame while the state is active
    public virtual void onStateTick() { return; }

    // Returns the next state the machine should switch to
    public virtual State onStateExit() { return this; }

    // Checks the conditions to switch the machines state
    public virtual bool checkStateSwitch() { return false; }
}