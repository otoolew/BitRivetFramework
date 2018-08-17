using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [CreateAssetMenu(fileName = "newEvent", menuName = "Events/OnSight")]
    public class OnSightEvent : ScriptableObject
    {

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<OnSightListener> eventListeners =
            new List<OnSightListener>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(OnSightListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(OnSightListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}