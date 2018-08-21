using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

[CreateAssetMenu(fileName = "patrolTask", menuName = "Actor/NPC Task/Patrol Task")]
public class NPCPatrolTask : NPCTask {
    public override void PerformTask(NPCController performer)
    {
        performer.Patrol();
    }
}
