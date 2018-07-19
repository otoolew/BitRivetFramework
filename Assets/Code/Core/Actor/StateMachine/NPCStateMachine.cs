using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class NPCStateMachine : StateMachineBehaviour
    {
        public NPCActor actor;
        public ActorInfo actorInfo;

        private NPCMovement npcMovement;
        public NPCMovement NPCMovement
        {
            get { return npcMovement; }
            private set { npcMovement = value; }
        }


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            actor = animator.gameObject.GetComponent<NPCActor>();
            npcMovement = animator.gameObject.GetComponent<NPCMovement>();
            actorInfo = animator.gameObject.GetComponent<ActorInfo>();
        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {


        }
    }
}
