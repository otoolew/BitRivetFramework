﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class NPCMoveState : StateMachineBehaviour
    {
        NPCController controller;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            controller = animator.GetComponent<NPCController>();
            controller.stateStatus = "Moving";
            Debug.Log("NPC State: " + controller.stateStatus);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //NPCMovement.NavAgent.SetDestination(NPCMovement.Destination);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
