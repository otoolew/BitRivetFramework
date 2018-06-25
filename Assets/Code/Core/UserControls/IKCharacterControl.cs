// ----------------------------------------------------------------------------
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
        Vector3 capsuleCenter;
        // World Orientation
        float forwardAmount;
        float turnAmount;
        // Character Stats
        public float crouchSpeed = 2.0f;
        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float sprintSpeed = 10.0f;
        public bool aiming;
        public bool crouching;
        public float rotationSpeed = 0.15f;
        public float gravity = 20.0f;
        public Transform aimPoint;
        public LayerMask layerMask;


        // Use this for initialization
        void Start () 
		{
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            aimIK = GetComponent<AimIK>();
            cameraTransform = Camera.main.transform;
            capsuleHeight = controller.height;
            capsuleCenter = controller.center;
        }

        // Update is called once per frame
        void Update()
        {
            float currentSpeed = runSpeed;
            //float moveState = 0;
            //aiming = false;
            if (controller.isGrounded)
            {
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");
                moveDirection = new Vector3(moveX, 0.0f, moveY);

                if (cameraTransform != null)
                {
                    cameraForward = Vector3.Scale(cameraTransform.up, new Vector3(1, 0, 1)).normalized;
                    moveAnimation = moveY * cameraForward + moveX * cameraTransform.right;
                }
                else
                {
                    moveAnimation = moveY * Vector3.forward + moveX * Vector3.right;
                }
                if (moveAnimation.sqrMagnitude > 1)
                {
                    moveAnimation.Normalize();
                }

                if (Input.GetButton("Aim"))
                {
                    aimIK.enabled = true;
                    AimPoint();
                    currentSpeed = walkSpeed;
                    aiming = true;               
                }
                else
                {
                    aimIK.enabled = false;
                    aiming = false;
                }

                if (Input.GetButton("Crouch"))
                {
                    //controller.center = new Vector3(0, 0.9f, 0);
                    //controller.height = 1.5f;             
                    crouching = true;
                }
                else
                {
                    //controller.center = new Vector3(0,1.25f,0);
                    //controller.height = 2.25f;
                    crouching = false;
                }

                if (Input.GetButton("Sprint"))
                {
                    aimIK.enabled = false;
                    currentSpeed = sprintSpeed;
                    //moveState = 1.5f;
                }
                MovementAnimation(moveAnimation);
            }
            LegRotation(aiming);
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime * currentSpeed);
            //Debug.Log("Movement Speed: "+ currentSpeed);
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
            animator.SetBool("Aiming", aiming);
            animator.SetBool("Crouching", crouching);
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