using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopCommand : MonoBehaviour
{
    public Text TroopName;
    public NPCController controller;


    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
    public void AssignTroop()
    {
        TroopName.text = controller.NPCName;
    }

    public void CommandPatrol()
    {
        Debug.Log(controller.NPCName + " is Patroling");
    }
}
