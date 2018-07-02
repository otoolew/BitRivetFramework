using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class DummyDetect : MonoBehaviour
    {
        LineRenderer lineRenderer;
        // Use this for initialization
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {

            }
        }
    }
}

