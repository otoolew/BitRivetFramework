using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class TDLinearControl : MonoBehaviour
    {

        public float movementSpeed;
        public float turnSpeed;
        public float lookSpeed;

        void Update()
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

            if (Input.GetMouseButton(1))
            {
                Aiming();
            }
            else if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), turnSpeed);
            }

        }

        void Aiming()
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            }
        }
    }
}
