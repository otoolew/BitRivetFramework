using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    public ActorStats ActorStats;
    public abstract void HandleDeath();
}
