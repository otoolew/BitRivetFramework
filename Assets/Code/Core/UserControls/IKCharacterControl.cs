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
        CharacterController controller;
        Animator animator;
        AimIK aimIK;
        Ray ray;
        RaycastHit rayHit = new RaycastHit();
        Vector3 moveDirection = Vector3.zero;
        public float crouchSpeed = 2.0f;
        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float sprintSpeed = 10.0f;
        public bool aiming;
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
        }
		
		// Update is called once per frame
		void Update () 
		{
            float currentSpeed = runSpeed;
            float moveState = 0;
            //aiming = false;
            if (controller.isGrounded)
            {
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");
                moveDirection = new Vector3(moveX, 0.0f, moveY);
                if (moveDirection.sqrMagnitude > 0)
                    moveState = 1f;

                if (Input.GetButton("Aim"))
                {
                    aimIK.enabled = true;
                    AimPoint();
                    currentSpeed = walkSpeed;
                    if(moveDirection.sqrMagnitude > 0)
                        moveState = 0.5f;
                    aiming = true;
                }
                else
                {
                    aimIK.enabled = false;
                    aiming = false;
                }

                if (Input.GetButton("Crouch"))
                {
                    //Debug.Log("Crouch Not Implemented!");
                }

                if (Input.GetButton("Sprint"))
                {
                    aimIK.enabled = false;
                    currentSpeed = sprintSpeed;
                    moveState = 1.5f;
                }
                MovementAnimation(moveDirection.sqrMagnitude, moveState);
            }
            Turn(aiming);
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime * currentSpeed);
            Debug.Log("Movement Speed: "+ currentSpeed);
        }

        private void MovementAnimation(float moving, float moveState)
        {
            animator.SetFloat("MovementState", moveState);
        }
        void Turn(bool aiming)
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