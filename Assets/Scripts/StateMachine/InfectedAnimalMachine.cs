using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalWander : State
{
    InfectedAnimal animal;
    int waypointTarget = 0;
    Transform playerTransform;
    NavMeshAgent agent;

    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        animal = stateContext.GetComponent<InfectedAnimal>();
        playerTransform = animal.playerTransform;
        agent = stateContext.GetComponent<NavMeshAgent>();

        agent.SetDestination(animal.waypoints[0].position);
        agent.speed = animal.walkSpeed;
    }

    public override void onStateTick()
    {
        if (animal.waypoints.Length > 0)
        {
            float distanceToWaypoint = Vector3.Distance(stateContext.transform.position, animal.waypoints[waypointTarget].position);
            if (distanceToWaypoint <= animal.waypointStopDistance)
            {
                getNextWaypoint();
            }
        }
    }

    public override bool checkStateSwitch()
    {
        var shouldChase = false;

        if (playerTransform)
        {
            float distance = Vector3.Distance(stateContext.transform.position, playerTransform.position);

            if (distance <= animal.noticeRange)
            {
                animal.visionIndicator.SetActive(true);
            }
            else
            {
                animal.visionIndicator.SetActive(false);
            }

            if (distance <= animal.chaseDistance)
            {
                nextState = new Chase();
                shouldChase = true;
            }
        }

        return shouldChase;
    }

    private void getNextWaypoint()
    {
        waypointTarget = (waypointTarget + 1) % animal.waypoints.Length;
        agent.SetDestination(animal.waypoints[waypointTarget].position);
    }
}


public class InfectedAnimalMachine : MachineType
{
    public InfectedAnimalMachine()
    {
        possibleStates = new List<State>
        {
            new AnimalWander()
        };
    }
}
