using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageZone : MonoBehaviour
{
    public HealthController healthController;

    public void HandleDamageZoneHit(float damage)
    {
        //Debug.Log("[DamageZone] Handling Hit");
        healthController.TakeDamage(damage);
    }
}
