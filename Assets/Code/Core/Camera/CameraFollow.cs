// ----------------------------------------------------------------------------
// Project: Core
// Date: 11 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
	public class CameraFollow : MonoBehaviour 
	{
        public GameObject Target;


        private Vector3 offset;

        // Use this for initialization
        void Start()
        {
            offset = transform.position - Target.transform.position;
        }

        void Update()
        {
            transform.position = Target.transform.position + offset;
        }
    }
}
