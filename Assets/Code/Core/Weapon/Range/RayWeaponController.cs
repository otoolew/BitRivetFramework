using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class RayWeaponController : MonoBehaviour
    {
        public RayCastLine rayLine;
        public KeyCode fireKey;
        public float FireRate;
        float timer;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;
            if (Input.GetKey(fireKey) && timer >= FireRate)
            {
                timer = 0;
                rayLine.Fire();
            }
        }
    }
}
