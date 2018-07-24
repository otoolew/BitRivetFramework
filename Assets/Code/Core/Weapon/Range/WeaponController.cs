using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class WeaponController : MonoBehaviour
    {
        public RayCastLine rayLine;

        public void FireRay()
        {
            rayLine.Fire();
        }
    }
}
