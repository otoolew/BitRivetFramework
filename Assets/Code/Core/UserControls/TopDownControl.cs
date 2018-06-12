// ----------------------------------------------------------------------------
// Author: William O'Toole
// Project: ProjectName
// Date: TimeStamp
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class TopDownControl : MonoBehaviour 
	{
        //private Animator animator;                              // Reference to the animator component.
        private Rigidbody playerRigidbody;                      // Reference to the player's rigidbody.
        private Vector3 movementInput;                          // The vector to store the direction of the player's movement.
        public float movementSpeed = 6f;                        // The speed that the player will move at.
        public float turnSpeed = 6f;
        private Vector3 movementVelocity;

        void Awake()
        {
            // Set up references.
            //animator = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            float hortinput = Input.GetAxisRaw("Horizontal");
            float vertinput = Input.GetAxisRaw("Vertical");

            movementInput = new Vector3(hortinput, 0f, vertinput);
            movementVelocity = movementInput * movementSpeed;
            //Animating(hortinput, vertinput);
            //float rayLength;
            if(Input.GetMouseButton(1))
                Aiming();
        }
        void FixedUpdate()
        {
            playerRigidbody.velocity = movementVelocity;
        }
        void Aiming()
        {
            // Generate a plane that intersects the transform's position with an upwards normal.
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            // Generate a ray from the cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.
            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            //   then find the point along that ray that meets that distance.  This will be the point
            //   to look at.
            float hitdist = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
        //void Animating(float h, float v)
        //{

        //    // Create a boolean that is true if either of the input axes is non-zero.
        //    bool walking = h != 0f || v != 0f;

        //    //animator.SetFloat("Movement", movementInput.z);
        //    // Tell the animator whether or not the player is walking.
        //    animator.SetBool("Moving", walking);
        //}
    
    }
}
