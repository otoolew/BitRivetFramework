using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Actions/Patrol")]
    public class PatrolAction : Action
    {
        public override void Act(NPCController controller)
        {
            Patrol(controller);
        }

        private void Patrol(NPCController controller)
        {
            controller.NPCMovement.NavAgent.destination = controller.NPCMovement.patrolPoints[controller.NPCMovement.nextWayPoint].position;
            controller.NPCMovement.NavAgent.isStopped= false;

            if (controller.NPCMovement.NavAgent.remainingDistance <= controller.NPCMovement.NavAgent.stoppingDistance && !controller.NPCMovement.NavAgent.pathPending)
            {
                controller.NPCMovement.nextWayPoint = (controller.NPCMovement.nextWayPoint + 1) % controller.NPCMovement.patrolPoints.Count;
            }
        }
    }
}
