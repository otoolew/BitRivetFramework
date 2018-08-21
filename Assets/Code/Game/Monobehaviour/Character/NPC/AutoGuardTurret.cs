using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGuardTurret : MonoBehaviour {

    public bool alertMode;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void EnterAlertMode()
    {
        alertMode = true;
    }
    public void ExitAlertMode()
    {
        alertMode = false;
    }
}
