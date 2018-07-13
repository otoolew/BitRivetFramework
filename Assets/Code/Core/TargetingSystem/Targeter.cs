using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class Targeter : MonoBehaviour
    {
        /// <summary>
        /// Time until Next Search
        /// </summary>
        public float SearchTimer;
        /// <summary>
        /// Timer
        /// </summary>
        private float timer = 0;
        /// <summary>
        /// Fires when a targetable enters the target collider
        /// </summary>
        public event Action<Targetable> targetEntersRange;

        /// <summary>
        /// Fires when a targetable exits the target collider
        /// </summary>
        public event Action<Targetable> targetExitsRange;

        /// <summary>
        /// Fires when an appropriate target is found
        /// </summary>
        public event Action<Targetable> acquiredTarget;

        /// <summary>
        /// Fires when the current target was lost
        /// </summary>
        public event Action lostTarget;

        /// <summary>
        /// The current targetables in the collider
        /// </summary>
        public List<Targetable> TargetsInRange = new List<Targetable>();


        [SerializeField]
        private Targetable currentTargetable;
        /// <summary>
        /// The current targetable
        /// </summary>
        public Targetable CurrentTargetable
        {
            get
            {
                return currentTargetable;
            }

            private set
            {
                currentTargetable = value;
            }
        }
        /// <summary>
        /// If there was a targetable in the last frame
        /// </summary>
        public bool HadTarget;
        /// <summary>
        /// Checks if any targets are destroyed and aquires a new targetable if appropriate
        /// </summary>
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= SearchTimer)
            {
                CurrentTargetable = GetNearestTargetable();
            }

            if (CurrentTargetable == null && TargetsInRange.Count > 0)
            {
                CurrentTargetable = GetNearestTargetable();
                if (CurrentTargetable != null)
                {
                    if (acquiredTarget != null)
                    {
                        acquiredTarget(CurrentTargetable);
                    }
                }
            }

            HadTarget = CurrentTargetable != null;
        }
        /// <summary>
        /// Clears the list of current targets and clears all events
        /// </summary>
        public void ResetTargetter()
        {
            TargetsInRange.Clear();
            CurrentTargetable = null;

            targetEntersRange = null;
            targetExitsRange = null;
            acquiredTarget = null;
            lostTarget = null;
        }

        /// <summary>
        /// Returns all the targets within the collider. This list must not be changed as it is the working
        /// list of the targetter. Changing it could break the targetter
        /// </summary>
        public List<Targetable> GetAllTargets()
        {
            return TargetsInRange;
        }

        /// <summary>
        /// Checks if the targetable is a valid target
        /// </summary>
        /// <param name="targetable"></param>
        /// <returns>true if targetable is vaild, false if not</returns>
        private bool IsTargetableValid(Targetable targetable)
        {
            if (targetable == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// On exiting the trigger, a valid targetable is removed from the tracking list.
        /// </summary>
        /// <param name="other">The other collider in the collision</param>
        private void OnTriggerExit(Collider other)
        {
            var targetable = other.GetComponent<Targetable>();
            if (!IsTargetableValid(targetable))
            {
                return;
            }

            TargetsInRange.Remove(targetable);
            if (targetExitsRange != null)
            {
                targetExitsRange(targetable);
            }
            if (targetable == CurrentTargetable)
            {
                OnTargetRemoved(targetable);
            }
            else
            {
                // Only need to remove if we're not our actual target, otherwise OnTargetRemoved will do the work above
                targetable.removed -= OnTargetRemoved;
            }
        }

        /// <summary>
        /// On entering the trigger, a valid targetable is added to the tracking list.
        /// </summary>
        /// <param name="other">The other collider in the collision</param>
        public void OnTriggerEnter(Collider other)
        {
            var targetable = other.GetComponent<Targetable>();
            if (!IsTargetableValid(targetable))
            {
                return;
            }
            targetable.removed += OnTargetRemoved;
            TargetsInRange.Add(targetable);
            if (targetEntersRange != null)
            {
                targetEntersRange(targetable);
            }
        }

        /// <summary>
        /// Returns the nearest targetable within the currently tracked targetables 
        /// </summary>
        /// <returns>The nearest targetable if there is one, null otherwise</returns>
        public Targetable GetNearestTargetable()
        {
            int length = TargetsInRange.Count;

            if (length == 0)
            {
                return null;
            }

            Targetable nearest = null;
            float distance = float.MaxValue;
            for (int i = length - 1; i >= 0; i--)
            {
                Targetable targetable = TargetsInRange[i];
                if (targetable == null || targetable.HealthController.IsDead)
                {
                    TargetsInRange.RemoveAt(i);
                    continue;
                }
                float currentDistance = Vector3.Distance(transform.position, targetable.Position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    nearest = targetable;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Fired by the agents died event or when the current target moves out of range,
        /// Fires the lostTarget event.
        /// </summary>
        private void OnTargetRemoved(Targetable target)
        {
            target.removed -= OnTargetRemoved;
            if (CurrentTargetable != null)
            {
                if (lostTarget != null)
                {
                    lostTarget();
                }
                HadTarget = false;
                TargetsInRange.Remove(CurrentTargetable);
                CurrentTargetable = null;
            }
            else //wasnt the current target, find and remove from targets list
            {
                for (int i = 0; i < TargetsInRange.Count; i++)
                {
                    if (TargetsInRange[i] == target)
                    {
                        TargetsInRange.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
