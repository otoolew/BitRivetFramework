using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "Actor/NPC/Actions/Idle")]
    public class IdleAction : Action
    {
        public override void Act(NPCController controller)
        {
            Idle(controller);
        }

        private void Idle(NPCController controller)
        {
            Debug.Log("Idling");
        }
    }
}
