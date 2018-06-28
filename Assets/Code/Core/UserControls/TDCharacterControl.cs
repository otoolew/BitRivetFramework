// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CharacterController))]
    public class TDCharacterControl : MonoBehaviour
    {
        public PlayerInput playerInput;
        public float movementSpeed = 6.0f;
        public float rotationSpeed = 0.15f;
        public float lookSpeed = 20f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        private Vector3 moveDirection = Vector3.zero;
        private CharacterController controller;
        private Ray ray;
        private RaycastHit rayHit = new RaycastHit();
        public Transform aimPoint;
        public LayerMask layerMask;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (controller.isGrounded)
            {
                //float moveHorizontal = Input.GetAxisRaw("Horizontal");
                //float moveVertical = Input.GetAxisRaw("Vertical");
                moveDirection = new Vector3(playerInput.MoveInput.x, 0.0f, playerInput.MoveInput.y);

                if (playerInput.AimInput)
                {
                    Aiming();
                }
                else if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), rotationSpeed);
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime * movementSpeed);
        }
        void Aiming()
        {
            //Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.z);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, 100, layerMask))
            {
                var lookPos = rayHit.point - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;
                Vector3 hitPoint = rayHit.point;
                Vector3 vector = new Vector3(0, 1, 0);
                var newPoint = hitPoint + vector;
                aimPoint.transform.LookAt(newPoint);
            }
        }
    }
}