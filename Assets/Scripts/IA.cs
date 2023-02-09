using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agentAI;
    public Transform[] destinationPoints;
    public int desiredDestination = 0;

    bool isWaiting = false;
    public float waitTime;
    float timePased = 0;
    // Start is called before the first frame update
    void Start()
    {
        agentAI = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) > 3)
        {
            Patroll();
        }
        else if(Vector3.Distance(transform.position, target.position) < 3)
        {
            Chase();
        }
        
        if(isWaiting)
        {
            if(timePased < waitTime)
            {
                timePased += Time.deltaTime;
            }
            else
            {
                isWaiting = false;
                timePased = 0;
            }
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
            isWaiting = true;
        }
    }

    void Chase()
    {
        agentAI.destination = target.position;
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
