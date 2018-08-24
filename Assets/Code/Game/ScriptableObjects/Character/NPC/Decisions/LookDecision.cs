using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Decisions/Look")]
    public class LookDecision : Decision
    {
        public override bool Decide(NPCController controller)
        {
            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(NPCController controller)
        {
            RaycastHit hit;

            Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.lookRange, Color.green);

            if (Physics.SphereCast(controller.eyes.position, controller.lookSphereCastRadius, controller.eyes.forward, out hit, controller.lookRange,LayerMask.NameToLayer("Detact"))
                && hit.collider.CompareTag("Player"))
            {
                controller.NPCVision.CurrentTarget = hit.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
