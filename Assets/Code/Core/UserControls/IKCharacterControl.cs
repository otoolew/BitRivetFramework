﻿// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using RootMotion.FinalIK;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AimIK))]
    public class IKCharacterControl : MonoBehaviour 
	{
        // Components
        public PlayerInput PlayerInput { get; private set; }
        CharacterController controller;
        Animator animator;
        AimIK aimIK;
        Ray ray;
        RaycastHit rayHit = new RaycastHit();
        Vector3 moveDirection = Vector3.zero;
        Vector3 moveAnimation;
        // Camera
        Transform cameraTransform;
        Vector3 cameraForward;

        float capsuleHeight;
        public float CapsuleHeight
        {
            get { return capsuleHeight; }
            private set { capsuleHeight = value; }
        }
        Vector3 capsuleCenter;
        public Vector3 CapsuleCenter
        {
            get { return capsuleCenter; } 
            private set { capsuleCenter = value; }
        }

        // World Orientation
        float forwardAmount;
        float turnAmount;
        // Character Stats
        public float crouchSpeed = 2.0f;
        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float rotationSpeed = 0.15f;
        public float gravity = 20.0f;
        public Transform aimPoint;
        public LayerMask layerMask;
        public DamageZone[] HitColliders;

        // Use this for initialization
        void Start () 
		{
            try
            {
                PlayerInput = FindObjectOfType<PlayerInput>();
            }
            catch (System.NullReferenceException)
            {
                throw new UnityException("There is no PlayerInput Script. Please add one.");
            }

            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            aimIK = GetComponent<AimIK>();
            cameraTransform = Camera.main.transform;
            capsuleHeight = controller.height;
            capsuleCenter = controller.center;
            HitColliders = GetComponentsInChildren<DamageZone>();
        }

        // Update is called once per frame
        void Update()
        {
            float currentSpeed = runSpeed;
            //float moveState = 0;
            //aiming = false;
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(PlayerInput.MoveInput.x, 0.0f, PlayerInput.MoveInput.y);

                if (cameraTransform != null)
                {
                    cameraForward = Vector3.Scale(cameraTransform.up, new Vector3(1, 0, 1)).normalized;
                    moveAnimation = PlayerInput.MoveInput.y * cameraForward + PlayerInput.MoveInput.x * cameraTransform.right;
                }
                else
                {
                    moveAnimation = PlayerInput.MoveInput.y * Vector3.forward + PlayerInput.MoveInput.x * Vector3.right;
                }

                if (moveAnimation.sqrMagnitude > 1)
                {
                    moveAnimation.Normalize();
                }

                // Player Aiming Check
                if (PlayerInput.AimInput)
                {
                    aimIK.enabled = true;
                    AimPoint();
                    currentSpeed = walkSpeed;              
                }
                else
                {
                    aimIK.enabled = false;
                }
                //Crouch Speed Check
                if (PlayerInput.CrouchInput)             
                    currentSpeed = crouchSpeed;
                

                MovementAnimation(moveAnimation);
                LegRotation(PlayerInput.AimInput);
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime * currentSpeed);
        }

        private void MovementAnimation(Vector3 moveAnim)
        {
            if(moveAnim.magnitude > 1)
            {
                moveAnim.Normalize();
            }

            Vector3 localMovement = transform.InverseTransformDirection(moveAnim);
            turnAmount = localMovement.x;
            forwardAmount = localMovement.z;

            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.SetBool("Aiming", PlayerInput.AimInput);
            animator.SetBool("Crouching", PlayerInput.CrouchInput);
        }

        void LegRotation(bool aiming)
        {
            if (aiming)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = aimPoint.position - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed);
            }
            else
            {
                if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), rotationSpeed);
                }
            }
  
        }
        /// <summary>
        /// Aims characters head, torso, and weapon to a rayhit point 
        /// </summary>
        private void AimPoint()
        {
            //Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.z);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, 100, layerMask))
            {
                Vector3 hitPoint = rayHit.point;
                aimPoint.transform.position = hitPoint;

                Vector3 targetDir = hitPoint - transform.position;
                // The step size is equal to speed times frame time.
                float step = runSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                newDir.y = 0;
                Debug.DrawRay(transform.position, newDir, Color.red);
                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
}