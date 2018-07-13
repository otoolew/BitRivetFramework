using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class Targetable : MonoBehaviour
    {
        #region Fields / Properties
        [SerializeField]
        private Vector3 position;
        /// <summary>
        /// Returns our targetable's transform position
        /// </summary>
        public Vector3 Position
        {
            get { return TargetTransform.position; }
            private set { position = value; }
        }

        [SerializeField]
        private Transform targetTransform;
        /// <summary>
        /// The transform that will be targeted
        /// </summary>
        public Transform TargetTransform
        {
            get { return targetTransform ?? transform; }
            private set { targetTransform = value; }
        }
        #endregion

        #region Components
        public HealthController HealthController;
        //public HealthController HealthController { get; private set; }
        #endregion

        #region Action / Events
        public event Action<Targetable> removed;

        #endregion
        private void Awake()
        {
            HealthController = GetComponent<HealthController>();
        }

        /// <summary>
        /// Fires the removed event
        /// </summary>
        void OnRemoved()
        {
            if (removed != null)
            {
                removed(this);
            }
        }
    }       
}
