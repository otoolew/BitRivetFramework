// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;
namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class TDRigidControl : MonoBehaviour
    {
        //private Animator animator;                              // Reference to the animator component.
        private Rigidbody rigidBody;                      // Reference to the player's rigidbody.
        private Vector3 movement;                          // The vector to store the direction of the player's movement.
        private Vector3 movementVelocity; 
        
        public float runSpeed = 6.0f;
        public float walkSpeed = 3.0f;
        public float crouchSpeed = 2.0f;
        public float sprintSpeed = 10.0f;
        public float turnSpeed = 0.15f;
        public float lookSpeed;

        public Transform aimPoint;
        public LayerMask layerMask;
        void Awake()
        {
            // Set up references.
            //animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            float hortinput = Input.GetAxisRaw("Horizontal");
            float vertinput = Input.GetAxisRaw("Vertical");

            movement = new Vector3(hortinput, 0f, vertinput);
            movementVelocity = movement * runSpeed;
            //Animating(hortinput, vertinput);
            //float rayLength;
            if (Input.GetMouseButton(1))
            {
                Debug.Log("Implement Aiming here");
            }
            else if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), turnSpeed);
            }
        }
        void FixedUpdate()
        {
            rigidBody.velocity = movementVelocity;
        }
 
    }
}
