using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Decisions/Scan")]
    public class ScanDecision : Decision
    {
        public override bool Decide(NPCController controller)
        {
            bool noEnemyInSight = Scan(controller);
            return noEnemyInSight;
        }

        private bool Scan(NPCController controller)
        {
            controller.NPCMovement.NavAgent.isStopped = true; 
            controller.transform.Rotate(0, 0.90f * Time.deltaTime, 0);
            return controller.CheckIfCountDownElapsed(controller.SearchTime);
        }
    }
}
