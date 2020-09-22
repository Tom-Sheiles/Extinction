using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wander : State
{
    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        Debug.Log("Context: " + context.name + " State: " + this.ToString());
    }
}
public class Chase : State { }

// All subclasses of MachineType are automatically added to state machine list
public class ZombieExample : MachineType
{
   public ZombieExample()
    {
        possibleStates = new List<State>
        {
            new Wander(),
            new Chase()
        };
    }
}


