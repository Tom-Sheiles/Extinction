using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : State
{
    Infected infected;
    NavMeshAgent agent;
    int waypointTarget = 0;
    Transform playerTransform;

    public override void onStateEnter(GameObject context)
    {
        this.stateContext = context;
        infected = context.GetComponent<Infected>();
        agent = context.GetComponent<NavMeshAgent>();
        playerTransform = infected.playerTransform;

        agent.speed = infected.walkingSpeed;
        agent.destination = infected.waypoints[waypointTarget].position;
    }

    public override void onStateTick()
    {
        float distanceToWaypoint = Vector3.Distance(stateContext.transform.position, infected.waypoints[waypointTarget].position);
        if(distanceToWaypoint <= infected.waypointStopDistance)
        {
            getNextWaypoint();
        }
    }

    public override bool checkStateSwitch()
    {
        Vector3 targetDir = playerTransform.position - stateContext.transform.position;
        float angle = Vector3.Angle(targetDir, stateContext.transform.forward);

        if(angle <= infected.visionAngle)
        {
            nextState = new Chase();
            return true;
        }

        return false;
    }

    private void getNextWaypoint()
    {
        waypointTarget = (waypointTarget + 1) % infected.waypoints.Length;
        agent.SetDestination(infected.waypoints[waypointTarget].position);
    }
}



public class Chase : State {

    Infected infected;
    NavMeshAgent agent;
    Transform playerTransform;

    float timeBetweenRecalc = 0.1f;
    float recalcuateDestTimer = 0;

    public override void onStateEnter(GameObject context)
    {
        this.stateContext = context;
        infected = context.GetComponent<Infected>();
        agent = context.GetComponent<NavMeshAgent>();
        playerTransform = infected.playerTransform;

        agent.SetDestination(playerTransform.position);
    }

    public override void onStateTick()
    {
        recalcuateDestTimer += Time.deltaTime;

        if(recalcuateDestTimer >= timeBetweenRecalc)
        {
            agent.SetDestination(playerTransform.position);
            recalcuateDestTimer = 0;
        }
    }
}

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


