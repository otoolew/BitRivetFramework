using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCTask : ScriptableObject
{
    public abstract void PerformTask(NPCController performer);
}
