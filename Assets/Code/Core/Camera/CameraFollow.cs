// ----------------------------------------------------------------------------
// Project: Core
// Date: 11 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
	public class CameraFollow : MonoBehaviour 
	{
        public Transform target;
        public CameraSettings cameraSettings;

        private void Start()
        {
            transform.Rotate(cameraSettings.tiltAngle);
        }
        private void LateUpdate()
        {
            transform.position = target.position + cameraSettings.offset;
        }
    }
}
