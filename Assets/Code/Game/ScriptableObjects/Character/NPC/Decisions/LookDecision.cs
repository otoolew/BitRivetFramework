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
            return controller.NPCVision.HasTarget;
        }
    }
}
