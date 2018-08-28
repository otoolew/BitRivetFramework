using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Decisions/ActiveState")]
    public class ActiveStateDecision : Decision
    {
        public override bool Decide(NPCController controller)
        {
            return controller.NPCVision.HasTarget;
        }
    }
}
