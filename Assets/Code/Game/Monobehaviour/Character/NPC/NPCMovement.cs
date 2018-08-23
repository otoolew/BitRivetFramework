using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    #region Components
    NavMeshAgent navAgent;
    Animator animator;
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;
    #endregion

    #region Fields / Properties      
    // Character Stats
    public float crouchSpeed = 2.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 0.15f;

    #endregion
    public float MoveVelocity;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = walkSpeed;
        navAgent.autoBraking = false;
        navAgent.destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveVelocity = navAgent.velocity.magnitude;
        MovementAnimation(MoveVelocity);
    }

    private void MovementAnimation(float velocity)
    {
        //MoveVelocity = navAgent.velocity.magnitude;
        animator.SetFloat("MoveVelocity", velocity);
    }
    public void ContinuePatrol()
    {
        if (!navAgent.isActiveAndEnabled)
            return;
        navAgent.isStopped = false;
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.1f)
            GotoNextWayPoint();
    }
    public void Stop()
    {
        if (!navAgent.isActiveAndEnabled)
            return;
        navAgent.SetDestination(transform.position);
        MoveVelocity = navAgent.velocity.magnitude;
    }
    public void GotoNextWayPoint()
    {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        navAgent.destination = patrolPoints[currentPatrolPoint].position;
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }
    public void GoToPosition(Vector3 position)
    {
        if (!navAgent.isActiveAndEnabled)
            return;

        navAgent.isStopped = false;
        navAgent.destination = position;
    }
    public void ClearNavAgentPath()
    {
        navAgent.ResetPath();
    }
    public void ActivateNavAgent()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.enabled = true;
    }
}

