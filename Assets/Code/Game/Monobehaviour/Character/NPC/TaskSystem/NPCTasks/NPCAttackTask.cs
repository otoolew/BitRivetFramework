using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTask", menuName = "Actor/NPC Task/Attack Task")]
public class NPCAttackTask : NPCTask
{
    public override void PerformTask(NPCController performer)
    {
        //performer.Attack();
    }
}
