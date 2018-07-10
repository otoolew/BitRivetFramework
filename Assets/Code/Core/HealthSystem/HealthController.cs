using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class HealthController : MonoBehaviour
    {
        public KeyCode killKey;
        public bool isDead;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(killKey))
            {
                isDead = true;
            }
        }
    }
}
