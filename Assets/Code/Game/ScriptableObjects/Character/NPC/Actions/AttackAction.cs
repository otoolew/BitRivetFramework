using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(NPCController controller)
        {
            Attack(controller);
        }

        private void Attack(NPCController controller)
        {
            RaycastHit hit;

            Debug.DrawRay(controller.eyes.position, controller.eyes.position * controller.NPCAttack.AttackRadius, Color.red);

            if (Physics.SphereCast(controller.eyes.position, controller.lookSphereCastRadius, controller.eyes.forward, out hit, controller.NPCAttack.AttackRadius)
                && hit.collider.CompareTag("Player"))
            {
                if (controller.CheckIfCountDownElapsed(controller.attackRate))
                {
                    controller.NPCAttack.Fire();
                }
            }
        }
    }
}
