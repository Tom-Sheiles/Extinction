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
    PlayerMovement playerMovement;
    float seenTimer = 0;

    public override void onStateEnter(GameObject context)
    {
        stateContext = context;
        infected = context.GetComponent<Infected>();
        agent = context.GetComponent<NavMeshAgent>();
        playerTransform = infected.playerTransform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();

        agent.speed = infected.walkingSpeed;

        if(infected.waypoints.Length > 0)
            agent.destination = infected.waypoints[waypointTarget].position;
    }

    public override void onStateTick()
    {
        if (infected.waypoints.Length > 0)
        {
            float distanceToWaypoint = Vector3.Distance(stateContext.transform.position, infected.waypoints[waypointTarget].position);
            if (distanceToWaypoint <= infected.waypointStopDistance)
            {
                getNextWaypoint();
            }
        }
    }

    public override bool checkStateSwitch()
    {
        Vector3 targetDir = playerTransform.position - stateContext.transform.position;
        float angleToPlayer = Vector3.Angle(targetDir, stateContext.transform.forward);
        float distanceToPlayer = Vector3.Distance(stateContext.transform.position, playerTransform.position);

        // If player is within field of view
        if(angleToPlayer <= infected.visionAngle && distanceToPlayer <= infected.maxVisionDistance)
        {
            infected.visionIndicator.SetActive(true);
            //infected.visionImage.color = infected.alertColor;
            float seenIncrease = infected.distanceIncreaseRatio / distanceToPlayer;
            float seenMultiplier = 1f;

            if (playerMovement.isCrouching) seenMultiplier = infected.crouchingVisionmultiplier;
           
            seenTimer += (seenIncrease * seenMultiplier);

            //Debug.Log("Seen timer: " + seenTimer + " increasing at " + seenIncrease + " per second");

            // if the enemy has seen the player for enough time
            if(seenTimer >= infected.noticeTime)
            {
                infected.visionImage.color = infected.spottedColor;
                nextState = new Chase();
                return true;
            }
        }
        else
        {
            seenTimer = 0;
            agent.speed = infected.walkingSpeed;
            infected.visionIndicator.SetActive(false);
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

    IMelee melee;
    NavMeshAgent agent;
    Transform playerTransform;

    float timeBetweenRecalc = 0.1f;
    float recalcuateDestTimer = 0;

    float visionIndicatorTimeout = 1.0f;
    float visionTimer = 0;

    public override void onStateEnter(GameObject context)
    {
        stateContext = context;
        melee = context.GetComponent<IMelee>();
        agent = context.GetComponent<NavMeshAgent>();
        playerTransform = melee.getTargetTransform();

        agent.SetDestination(playerTransform.position);
        agent.speed = melee.getChaseSpeed();
    }

    public override void onStateTick()
    {
        recalcuateDestTimer += Time.deltaTime;
        visionTimer += Time.deltaTime;

        if(recalcuateDestTimer >= timeBetweenRecalc)
        {
            agent.SetDestination(playerTransform.position);
            recalcuateDestTimer = 0;
        }

        if(visionTimer >= visionIndicatorTimeout)
        {
            //melee.visionIndicator.SetActive(false);
        }
    }

    public override bool checkStateSwitch()
    {
        float dist = Vector3.Distance(stateContext.transform.position, playerTransform.position);
        if(dist <= melee.getAttackRange())
        {
            nextState = new MeleeAttack();
            return true;
        }

        return false;
    }
}



public class Dead : State
{
    NavMeshAgent agent;
    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        agent = context.GetComponent<NavMeshAgent>();
        agent.SetDestination(context.transform.position);
        context.SetActive(false);
    }
}


public class MeleeAttack : State
{
    IMelee melee;
    Transform playerTransform;
    float attackCooldown = 0;

    public override void onStateEnter(GameObject context)
    {
        base.onStateEnter(context);
        melee = context.GetComponent<IMelee>();
        playerTransform = melee.getTargetTransform();
    }

    public override void onStateTick()
    {

        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        else if(attackCooldown < 0)
        {
            attackCooldown = 0;
        }

        if(attackCooldown <= 0)
        {
            playerTransform.GetComponent<PlayerHealth>().takeDamage(melee.getDamage());
            attackCooldown = melee.getAttackSpeed();
        }
    }

    public override bool checkStateSwitch()
    {
        float dist = Vector3.Distance(stateContext.transform.position, playerTransform.position);
        if(dist > melee.getAttackRange())
        {
            nextState = new Chase();
            return true;
        }

        return false;
    }
}


// All subclasses of MachineType are automatically added to state machine list
public class InfectedHumanMachine : MachineType
{
   public InfectedHumanMachine()
    {
        possibleStates = new List<State>
        {
            new Wander(),
            new Chase(),
            new MeleeAttack(),
            new Dead()
        };
    }
}


