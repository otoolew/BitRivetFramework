using UnityEngine;
using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventSceneChangeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventStartDialogue : UnityEvent<bool> { }
    [System.Serializable] public class EventFinishDialogue : UnityEvent<bool> { }
}
