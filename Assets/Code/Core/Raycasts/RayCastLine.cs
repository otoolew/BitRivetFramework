using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class RayCastLine : MonoBehaviour
    {
        LineRenderer lineRenderer;
        Ray ray;
        RaycastHit rayHit;
        public float FireRate;
        public float RayRange;
        public LayerMask LayerRayMask;
        float timer;
        public float effectDuration = 0.1f;
        public UnityEvent OnRayHit;
        private void Start()
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        public void Fire()
        {
            lineRenderer.enabled = true;
            StartCoroutine(FireFX());
            lineRenderer.SetPosition(0, transform.position);
            ray.origin = transform.position;
            ray.direction = transform.forward;

            if (Physics.Raycast(ray, out rayHit, RayRange, LayerRayMask))
            {
                Debug.Log("Debug RayHit: " + rayHit.collider.name);
                OnRayHit.Invoke();

                lineRenderer.SetPosition(1, rayHit.point);
               
                //Debug.Log("Hit Success " + rayHit.collider.GetComponent<DamageBehaviour>().TakeDamage(10f));
            }
            else
            {
                lineRenderer.SetPosition(1, ray.origin + ray.direction * RayRange);
                Debug.Log("Debug RayHit: NOTHING");
            }          
        }
        IEnumerator FireFX()
        {
            yield return new WaitForSeconds(effectDuration);
            lineRenderer.enabled = false;
        }
    }
}
