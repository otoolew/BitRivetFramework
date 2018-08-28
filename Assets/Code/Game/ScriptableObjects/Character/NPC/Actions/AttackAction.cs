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
           
            if (controller.NPCAttack.InAttackRange)
            {
                controller.NPCMovement.Stop();
                controller.NPCAttack.LegRotation();
                controller.NPCAttack.Fire();
            }
        }
    }
}
