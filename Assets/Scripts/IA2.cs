using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA2 : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agentAI;
    public Transform[] destinationPoints;
    public int desiredDestination = 0;

    bool isWaiting = false;
    public float waitTime;
    float timePased = 0;

    enum State
    {
        Waiting,
        Patrolling,
        Chasing,
        Attacking
    }

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        agentAI = GetComponent<NavMeshAgent>();

        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Patrolling)
        {
            Patroll();
        }
        else if(currentState == State.Chasing)
        {
            Chase();
        }
        else if (currentState == State.Waiting)
        {
            Wait();
        }
        else if( currentState == State.Attacking)
        {
            Attack();
        } 
    }

    void Patroll()
    {
        agentAI.destination = destinationPoints[desiredDestination].position;
        if(Vector3.Distance(transform.position, destinationPoints[desiredDestination].position) <= 1 && !isWaiting)
        {
            if(desiredDestination < destinationPoints.Length - 1)
            {
                desiredDestination++;
            }
            else
            {
                desiredDestination = 0;
            }
        }

        if(Vector3.Distance(transform.position, destinationPoints[desiredDestination].position) <= 1.5f)
        {
            currentState = State.Waiting;
        }

        if(Vector3.Distance(transform.position, target.position) < 5)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agentAI.destination = target.position;

        if(Vector3.Distance(transform.position, target.position) > 5)
        {
            currentState = State.Patrolling;
        }
        else if(Vector3.Distance(transform.position, target.position) < 2) 
        {
            currentState = State.Attacking;
        }
    }

    void Attack()
    {
        Debug.Log("Attacking");
        agentAI.Stop();
        
        currentState = State.Chasing;
    }

    void Wait()
    {
        if(timePased < waitTime)
        {
            timePased += Time.deltaTime;
        }
        else
        {
            timePased = 0;
            currentState = State.Patrolling;
        }
    }

    void OnDrawGizmos()
    {
        foreach (Transform destination in destinationPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(destination.position, 0.5f);
        }
    }
}
