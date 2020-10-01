using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : State
{
    Transform playerTransform;
    Boss boss;

    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        boss = stateContext.GetComponent<Boss>();

        playerTransform = boss.getTargetTransform();
    }

    public override bool checkStateSwitch()
    {
        float distance = Vector3.Distance(stateContext.transform.position, playerTransform.position);

        if(distance <= boss.visionRange)
        {
            nextState = new Chase();
            return true;
        }

        return false;
    }
}



public class BossStateMachine : MachineType
{
   public BossStateMachine()
    {
        possibleStates = new List<State>
        {
            new BossIdle(),
            new Chase(),
            new MeleeAttack()
        };
    }
}
