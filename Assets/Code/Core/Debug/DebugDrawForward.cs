using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
	public class DebugDrawForward : MonoBehaviour 
	{
        public float drawDistance;
	
		// Update is called once per frame
		void Update () {
            Debug.DrawRay(transform.position, transform.forward.normalized * drawDistance, Color.red);
        }
	}
}
