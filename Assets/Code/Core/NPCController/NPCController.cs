
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCController : MonoBehaviour 
	{

        [SerializeField]
        public PlayerInfo PlayerInfo { get; private set; }
        [SerializeField]
        private readonly float distanceToPlayer;
        public float DistanceToPlayer
        {
            get
            {
                return Vector3.Distance(transform.position,PlayerInfo.PlayerPosition);
            }

        }

        // Use this for initialization
        void Start () {
         
        }
        private void OnEnable()
        {
            PlayerInfo = FindObjectOfType<PlayerInfo>();
        }
        // Update is called once per frame
        void Update () {
			
		}
	}
}

