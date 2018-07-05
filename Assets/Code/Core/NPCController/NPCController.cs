using Game;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AimIK))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCController : MonoBehaviour 
	{
        public VisionDetection VisionDetection { get; private set; }
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
            VisionDetection = GetComponent<VisionDetection>();
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

