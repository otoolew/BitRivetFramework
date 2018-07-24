using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AimIK))]
    public class NPCActor : MonoBehaviour
    {
        Animator animator;
        AimIK aimIK;
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            aimIK = GetComponent<AimIK>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
