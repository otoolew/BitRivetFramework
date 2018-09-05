using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour {
    public Transform InteractionPoint;
    public bool InUse;
    public float InteractTime;
    public abstract void Interact();
}
