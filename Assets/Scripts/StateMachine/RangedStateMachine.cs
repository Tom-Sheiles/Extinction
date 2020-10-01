using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedStateMachine : MachineType
{
    public RangedStateMachine()
    {
        possibleStates = new List<State>
        {
            new Wander(),
            new Chase(),
            new MeleeAttack()
        };
    }
}
