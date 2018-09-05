using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCmdTrigger : MonoBehaviour
{
    public NPCController controller;
    private TroopCommand cmdUI;

    public TroopCommand CmdUI
    {
        get { return cmdUI; }
        private set { cmdUI = value; }
    }
    // Use this for initialization
    void Start ()
    {
        cmdUI = FindObjectOfType<TroopCommand>();
    }
	
    public void OnMouseOver()
    {
        cmdUI.controller = controller;
        cmdUI.AssignTroop();
    }
}
