using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Line")]
    public class DialogueData : ScriptableObject 
	{
        public List<DialogueLineData> DialogueLineList;
    }
}
