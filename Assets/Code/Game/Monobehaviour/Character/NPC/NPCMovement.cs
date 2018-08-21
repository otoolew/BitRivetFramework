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
    #endregion

    #region Fields / Properties      
    // Character Stats
    public float crouchSpeed = 2.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 0.15f;

    /// <summary>
    /// Returns our targetable's transform position
    /// </summary>
    public Vector3 Position
    {
        get { return transform.position; }
    }

    [SerializeField]
    private Vector3 destination;
    /// <summary>
    /// Returns NPC Destination
    /// </summary>
    public Vector3 Destination
    {
        get { return destination; }
        set { destination = value; }
    }
    public Transform TargetPoint;
    #endregion
    public float MoveVelocity;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        destination = TargetPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
        destination = TargetPoint.position;
        //animator.SetFloat("MovementVelocity", navAgent.velocity.magnitude);
    }
    private void MovementAnimation()
    {
        MoveVelocity = navAgent.velocity.magnitude;
        animator.SetFloat("MoveVelocity", MoveVelocity);
        //if (navAgent.velocity.magnitude > 0)
        //{
        //    animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        //}

        //Vector3 localMovement = transform.InverseTransformDirection(moveAnim);
        //turnAmount = localMovement.x;
        //forwardAmount = localMovement.z;

        //animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        //animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
        //animator.SetBool("Aiming", PlayerInput.AimInput);
        //animator.SetBool("Crouching", PlayerInput.CrouchInput);
    }
}

