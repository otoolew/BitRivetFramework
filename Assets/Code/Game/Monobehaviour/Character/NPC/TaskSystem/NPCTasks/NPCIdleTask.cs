using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "idleTask", menuName = "Actor/NPC Task/Idle Task")]
public class NPCIdleTask : NPCTask {
    public override void PerformTask(NPCController performer)
    {
        performer.Idle();
    }
}
