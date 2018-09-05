using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Interactive[] InteractiveObjects;
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AssignInteraction(NPCController controller)
    {
        controller.NPCMovement.NavAgent.SetDestination(InteractiveObjects[0].InteractionPoint.position);
    }
}
