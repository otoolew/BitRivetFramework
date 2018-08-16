using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCMovement : MonoBehaviour
    {
        #region Fields / Properties      
        // Character Stats
        public float crouchSpeed = 2.0f;
        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float rotationSpeed = 0.15f;



        [SerializeField]
        private Vector3 position;
        /// <summary>
        /// Returns our targetable's transform position
        /// </summary>
        public Vector3 Position
        {
            get { return transform.position; }
            private set { position = value; }
        }

        [SerializeField]
        private Vector3 destination;
        /// <summary>
        /// Returns NPC Destination
        /// </summary>
        public Vector3 Destination
        {
            get { return destination; }
            set { destination = value; }
        }


        #endregion
        #region Components
        NavMeshAgent navAgent;
        public NavMeshAgent NavAgent
        {
            get { return navAgent; }
            private set { navAgent = value; }
        }
        Animator animator;
        public Transform TargetPoint;

        #endregion

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            navAgent = GetComponent<NavMeshAgent>();
            destination = TargetPoint.position;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
