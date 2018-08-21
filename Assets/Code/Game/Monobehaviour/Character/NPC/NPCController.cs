
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCController : ActorController 
	{
        private NPCMovement npcMovement;
        public NPCMovement NPCMovement
        {
            get { return npcMovement; }
            private set { npcMovement = value; }
        }
        [SerializeField]
        public PlayerController PlayerController { get; private set; }

        [SerializeField]
        private readonly float distanceToPlayer;
        public float DistanceToPlayer
        {
            get
            {
                return Vector3.Distance(transform.position,PlayerController.PlayerPosition);
            }

        }

        // Use this for initialization
        void Start ()
        {
            PlayerController = FindObjectOfType<PlayerController>();
        }


        public override void HandleDeath()
        {
            Debug.Log(gameObject.name + " is dead.");
        }
    }
}

