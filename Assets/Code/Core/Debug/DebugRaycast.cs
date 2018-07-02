using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
	public class DebugRaycast : MonoBehaviour 
	{
        //LineRenderer lineRenderer;
        Ray ray;
        RaycastHit rayHit;
        public float FireRate;
        public float RayRange;
        public LayerMask LayerRayMask;
        float timer;
        float effectDuration = 1f;
        public KeyCode fireKey;
        public UnityEvent OnRayHit;

        private void Start()
        {
            //lineRenderer = GetComponent<LineRenderer>();
        }
        void Update()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the Fire1 button is being press and it's time to fire...
            if (Input.GetKeyDown(fireKey) && timer >= FireRate)
            {
                Fire();
            }
        }
        private void Fire()
        {
            timer = 0f;
            ray.origin = transform.position;
            ray.direction = transform.forward;

            if (Physics.Raycast(ray, out rayHit, RayRange, LayerRayMask))
            {
                Debug.Log("Debug RayHit: " + rayHit.collider.name);
                //rayHit.collider.GetComponent<HitCollider>().TakeDamage(10f);
                //Debug.Log("Hit Success ");
            }
            else
            {
                Debug.Log("Debug RayHit: NOTHING");
            }
        }
    }
}
