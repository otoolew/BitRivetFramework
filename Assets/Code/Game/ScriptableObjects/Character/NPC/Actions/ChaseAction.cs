using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Actions/Chase")]
    public class ChaseAction : Action
    {
        public override void Act(NPCController controller)
        {
            Chase(controller);
        }

        private void Chase(NPCController controller)
        {
            controller.NPCMovement.NavAgent.destination = controller.PlayerController.PlayerPosition;
            controller.NPCMovement.NavAgent.isStopped = false;
        }
    }
}
