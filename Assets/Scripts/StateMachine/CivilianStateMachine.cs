using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CivilianWander : State
{
    Civilian civilian;
    NavMeshAgent agent;

    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        civilian = stateContext.GetComponent<Civilian>();
        agent = stateContext.GetComponent<NavMeshAgent>();
    }

    public override void onStateTick()
    {
        var colliders = Physics.OverlapSphere(stateContext.transform.position, civilian.visionRadius);

        foreach(var collider in colliders)
        {
            if(collider.CompareTag("EnemyBody"))
            {
                furthestWaypoint();
            }
        }
    }

    public void furthestWaypoint()
    {
        var waypoints = civilian.waypoints.OrderBy(x => Vector3.Distance(stateContext.transform.position, x.position)).ToList();
        agent.SetDestination(waypoints[waypoints.Count - 1].position);
    }
}

public class CivilianStateMachine : MachineType
{
    public CivilianStateMachine()
    {
        possibleStates = new List<State>
        {
            new CivilianWander()
        };
    }
}
