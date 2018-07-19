using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class ActorInfo : MonoBehaviour
    {
        [SerializeField]
        readonly Vector3 actorPosition;
        public Vector3 ActorPosition
        {
            get { return transform.position; }
        }

    }
}
