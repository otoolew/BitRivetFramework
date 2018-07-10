using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class TargetAgent : MonoBehaviour
    {
        /// <summary>
        /// What Team to target
        /// </summary>
        public string TargetTag;
        /// <summary>
        /// Event that is fired when this instance is removed, such as when pooled or destroyed
        /// </summary>
        public event Action<TargetAgent> removed;
        /// <summary>
        /// Event that is fired when this instance is killed
        /// </summary>
        public event Action<TargetAgent> died;

        /// <summary>
        /// Fires when a TroopAgent enters the target collider
        /// </summary>
        public event Action<TargetAgent> targetEntersRange;

        /// <summary>
        /// Fires when a TroopAgent exits the target collider
        /// </summary>
        public event Action<TargetAgent> targetExitsRange;

        /// <summary>
        /// Fires when an appropriate target is found
        /// </summary>
        public event Action<TargetAgent> acquiredTarget;

        /// <summary>
        /// Fires when the current target was lost
        /// </summary>
        public event Action lostTarget;

        /// <summary>
        /// The collider attached to the targetter
        /// </summary>
        public Collider AttachedCollider;

        /// <summary>
        /// The current targetables in the collider
        /// </summary>
        public List<TargetAgent> TargetsInRange = new List<TargetAgent>();


        /// <summary>
        /// The seconds until a search is allowed
        /// </summary>
        public float SearchTimer = 0.0f;

        /// <summary>
        /// The search rate in searches per second
        /// </summary>
        public float SearchRate;

        /// <summary>
        /// If there was a targetable in the last frame
        /// </summary>
        public bool HadTarget;

        /// <summary>
        /// The current targetable
        /// </summary>
        private TargetAgent currentTarget;
        public TargetAgent CurrentTarget
        {
            get
            {
                return currentTarget;
            }

            set
            {
                currentTarget = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            SearchTimer = SearchRate;
        }

        // Update is called once per frame
        void Update()
        {
            SearchTimer -= Time.deltaTime;

            if (SearchTimer <= 0.0f && CurrentTarget == null && TargetsInRange.Count > 0)
            {
                CurrentTarget = GetNearestTarget();
                if (CurrentTarget != null)
                {
                    if (acquiredTarget != null)
                    {
                        Debug.Log(gameObject.name + " acquired target " + CurrentTarget.name);
                        acquiredTarget(CurrentTarget);
                    }
                    SearchTimer = SearchRate;
                }
            }

            HadTarget = CurrentTarget != null;
        }
        /// <summary>
        /// Fired by the agents died event or when the current target moves out of range,
        /// Fires the lostTarget event.
        /// </summary>
        void onTargetRemoved(TargetAgent target)
        {
            //target.removed -= onTargetRemoved;
            if (CurrentTarget != null)
            {
                if (lostTarget != null)
                {
                    Debug.Log(gameObject.name + " lost target");
                    lostTarget();
                }
                HadTarget = false;
                TargetsInRange.Remove(CurrentTarget);
                CurrentTarget = null;
            }
            else //wasnt the current target, find and remove from targets list
            {
                for (int i = 0; i < TargetsInRange.Count; i++)
                {
                    if (TargetsInRange[i].Equals(target))
                    {
                        TargetsInRange.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Removes this agent from other agents target list when removed.
        /// </summary>
        /// <param name="targetAgent"></param>
        public void onTargetDestroyed(TargetAgent targetAgent)
        {
            if (CurrentTarget == targetAgent)
            {
                CurrentTarget.removed -= onTargetDestroyed;
                CurrentTarget = null;
            }
        }
        /// <summary>
        /// Removes this target without killing it
        /// </summary>
        public void Remove()
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.removed -= onTargetDestroyed;
            }
            CurrentTarget = null;
            onRemoved();
        }
        ///// <summary>
        ///// Fires the removed event
        ///// </summary>
        void onRemoved()
        {
            if (removed != null)
            {
                removed(this);
            }
        }
        ///// <summary>
        ///// The logic for what happens when the target dies
        ///// </summary>
        public void onDeath()
        {
            if (died != null)
            {
                died(this);
            }
        }
        /// <summary>
        /// Returns the nearest targetable within the currently tracked targetables 
        /// </summary>
        /// <returns>The nearest targetable if there is one, null otherwise</returns>
        private TargetAgent GetNearestTarget()
        {
            int length = TargetsInRange.Count;

            if (length == 0)
            {
                return null;
            }

            TargetAgent nearest = null;
            float distance = float.MaxValue;
            for (int i = length - 1; i >= 0; i--)
            {
                TargetAgent targetable = TargetsInRange[i];
                HealthController targetHealth = TargetsInRange[i].GetComponent<HealthController>();
                if (targetable == null || targetHealth.isDead)
                {
                    TargetsInRange.Remove(targetable);
                    //TargetsInRange.RemoveAt(i);
                    continue;
                }
                float currentDistance = Vector3.Distance(transform.position, targetable.transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    nearest = targetable;
                }
            }

            return nearest;
        }
        /// <summary>
        /// Returns all the targets within the collider. This list must not be changed as it is the working
        /// list of the targetter. Changing it could break the targetter
        /// </summary>
        public List<TargetAgent> GetAllTargets()
        {
            return TargetsInRange;
        }
        /// <summary>
        /// Clears the list of current targets and clears all events
        /// </summary>
        public void ResetTargetList()
        {
            TargetsInRange.Clear();
            CurrentTarget = null;
            targetEntersRange = null;
            targetExitsRange = null;
            acquiredTarget = null;
            lostTarget = null;
        }
        /// <summary>
        /// On entering the trigger, a valid targetable is added to the tracking list.
        /// </summary>
        /// <param name="other">The other collider in the collision</param>
        private void OnTriggerEnter(Collider other)
        {
            TargetAgent targetAgent = other.gameObject.GetComponent<TargetAgent>();

            if (targetAgent == null)
            {
                return;
            }

            if (targetAgent.tag == TargetTag)
            {
                targetAgent.removed += onTargetRemoved;
                TargetsInRange.Add(targetAgent);
                if (targetEntersRange != null)
                {
                    targetEntersRange(targetAgent);
                }
            }
        }
        /// <summary>
        /// On exiting the trigger, a valid targetable is removed from the tracking list.
        /// </summary>
        /// <param name="other">The other collider in the collision</param>
        protected void OnTriggerExit(Collider other)
        {
            //Debug.Log(other.name + " Exited");
            var targetAgent = other.GetComponent<TargetAgent>();
            //if (!IsTargetableValid(targetable))
            //{
            //    return;
            //}

            TargetsInRange.Remove(targetAgent);
            if (targetExitsRange != null)
            {
                targetExitsRange(targetAgent);
            }
            if (targetAgent == CurrentTarget)
            {
                onTargetRemoved(targetAgent);
            }
            else
            {
                // Only need to remove if we're not our actual target, otherwise OnTargetRemoved will do the work above
                //targetAgent.removed -= onTargetRemoved;
            }
        }
    }
}
