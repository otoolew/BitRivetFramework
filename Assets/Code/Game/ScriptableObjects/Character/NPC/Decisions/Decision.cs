using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(NPCController controller);
    }
}
