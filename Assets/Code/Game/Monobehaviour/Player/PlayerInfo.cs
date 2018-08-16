using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class PlayerInfo : MonoBehaviour 
	{

        [SerializeField]
        readonly Vector3 playerPosition;
        public Vector3 PlayerPosition
        { get { return transform.position; }}

        // Use this for initialization
        void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
