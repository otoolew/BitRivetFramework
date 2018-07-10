using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class DebugDrawSphere : MonoBehaviour 
	{
        public float sphereCastDistance;
        public float sphereCastRadius;
        public string tagCheck;
        public LayerMask layerMask;
        public OnSightListener onSightListener;
        private void Start()
        {
            onSightListener = GetComponent<OnSightListener>();
        }
        // Update is called once per frame
        void Update () {
            DrawSphereForward();

        }
        public void DrawSphereForward()
        {
            RaycastHit rayHit;

            Debug.DrawRay(transform.position, transform.forward.normalized * sphereCastDistance, Color.red);
           

            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out rayHit, sphereCastDistance, layerMask))
            {
                Debug.Log("Layer " + layerMask.ToString() + " was hit.");
                onSightListener.OnEventRaised();
                if (rayHit.collider.tag == tagCheck)
                {
                    Debug.Log("Tag " +tagCheck + " is in sight.");
                }              
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sphereCastRadius);
        }
    }
}
