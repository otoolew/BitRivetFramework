using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(NPCController controller);
    }
}

