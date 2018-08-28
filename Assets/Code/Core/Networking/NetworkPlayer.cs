using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkPlayer : NetworkBehaviour
{
    public GameObject PlayerUnitPrefab;

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {

        }
        else
        {
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    void CmdSpawnUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        NetworkServer.Spawn(go);
    }
}
