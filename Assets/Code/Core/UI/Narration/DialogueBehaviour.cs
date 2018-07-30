using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class DialogueBehaviour : MonoBehaviour 
	{
        public float playerTalkRange;
        public NPCController controller;
        public void Awake()
        {
            controller = GetComponent<NPCController>();
        }
        public void OnEnable()
        {

        }
        private void OnMouseOver()
        {
            playerTalkRange = controller.DistanceToPlayer;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Start Dialogue");
            }
        }

    }
}
