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
    [RequireComponent(typeof(AimIK))]
    public class IKCharacterControl : MonoBehaviour 
	{
        CharacterController controller;
        AimIK aimIK;
        Ray ray;
        RaycastHit rayHit = new RaycastHit();
        Vector3 moveDirection = Vector3.zero;
        public float movementSpeed = 6.0f;
        public float rotationSpeed = 0.15f;
        public float jumpSpeed = 4.0f;
        public float gravity = 20.0f;
        public Transform aimPoint;
        public LayerMask layerMask;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            aimIK = GetComponent<AimIK>();
        }
        // Use this for initialization
        void Start () 
		{
			
		}
		
		// Update is called once per frame
		void Update () 
		{
            if (controller.isGrounded)
            {
                float moveHorizontal = Input.GetAxisRaw("Horizontal");
                float moveVertical = Input.GetAxisRaw("Vertical");
                moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);

                if (Input.GetMouseButton(1))
                {
                    aimIK.enabled = true;
                    AimPoint();
                }
                else
                {
                    aimIK.enabled = false;
                }

                if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), rotationSpeed);
                }

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }

                if (Input.GetButton("Crouch"))
                {

                }
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), rotationSpeed);
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime * movementSpeed);
        }
        void AimPoint()
        {
            //Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.z);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, 100, layerMask))
            {
                Vector3 hitPoint = rayHit.point;
                aimPoint.transform.position = hitPoint;

                Vector3 targetDir = hitPoint - transform.position;
                // The step size is equal to speed times frame time.
                float step = movementSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                newDir.y = 0;
                Debug.DrawRay(transform.position, newDir, Color.red);
                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
}