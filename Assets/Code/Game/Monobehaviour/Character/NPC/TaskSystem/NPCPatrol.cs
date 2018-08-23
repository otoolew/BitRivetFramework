using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent navAgent;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        navAgent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        navAgent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}