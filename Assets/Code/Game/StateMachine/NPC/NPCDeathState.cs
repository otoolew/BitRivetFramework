﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeathState : StateMachineBehaviour
{
    NPCController controller;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<NPCController>();

        controller.NPCMovement.NavAgent.SetDestination(controller.transform.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (controller.DistanceToPlayer < controller.NPCAttack.AttackRange)
        {
            controller.NPCAttack.AimAtTarget();
            controller.NPCAttack.Fire();
        }
        else
        {
            if (controller.NPCMovement.NavAgent.isActiveAndEnabled)
            {
                controller.NPCMovement.NavAgent.isStopped = false;
                controller.NPCMovement.NavAgent.SetDestination(controller.PlayerController.transform.position);
            }

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
