// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using RootMotion.FinalIK;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AimIK))]
    public class IKRigidContol : MonoBehaviour 
	{
        Animator animator;
        AimIK aimIK;
        Ray ray;
        RaycastHit rayHit = new RaycastHit();
        Vector3 moveDirection = Vector3.zero;
        Rigidbody rigidBody;
        Vector3 movement;
        Vector3 movementVelocity;

        public float crouchSpeed = 2.0f;
        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float sprintSpeed = 10.0f;
        public bool aiming = false;
        public float rotationSpeed = 0.15f;
        public float gravity = 20.0f;
        public Transform aimPoint;
        public LayerMask layerMask;

        // Use this for initialization
        void Start () 
		{
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            aimIK = GetComponent<AimIK>();
        }
		
		// Update is called once per frame
		void Update () 
		{
            float hortinput = Input.GetAxisRaw("Horizontal");
            float vertinput = Input.GetAxisRaw("Vertical");
            //float moveState = 0;
            aiming = false;
            movement = new Vector3(hortinput, 0f, vertinput);
 
            movementVelocity = movement * runSpeed;
            if (Input.GetButton("Aim"))
            {
                aimIK.enabled = true;
                aiming = true;
                AimPoint();
                movementVelocity = movement * walkSpeed;
                Turn(aiming);

            }
            else
            {
                aimIK.enabled = false;
                aimPoint.localPosition = new Vector3(0, 2, 1);
            }

            if (Input.GetButton("Crouch"))
            {
                movementVelocity = movement * crouchSpeed;
            }

            if (Input.GetButton("Sprint"))
            {
                aimIK.enabled = false;
                movementVelocity = movement * sprintSpeed;
            }
            
        }
        private void FixedUpdate()
        {
            Move(movementVelocity);
            Turn(aiming);
        }
        private void Move(Vector3 movementVelocity)
        {
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            rigidBody.MovePosition(transform.position + movement);
        }
        private void Turn(bool aiming)
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
                rigidBody.MoveRotation(newRotation);
            }
            else
            {
                if (movement != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
                }
            }
        }
        private void AimPoint()
        {
            //Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.z);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, 100, layerMask))
            {
                Vector3 hitPoint = rayHit.point;
                aimPoint.transform.position = hitPoint;

                Vector3 targetDir = hitPoint - transform.position;
                Quaternion newRotation = Quaternion.LookRotation(targetDir);
                rigidBody.MoveRotation(newRotation);
            }
        }
    }
}