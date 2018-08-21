using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public ActorStats ActorStats;

    [SerializeField]
    private float currentHealth;
    /// <summary>
    /// The current health of this instance
    /// </summary>
    public float CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = value; }
    }

    private bool isDead;
    /// <summary>
    /// Is the intance dead
    /// </summary>
    public bool IsDead
    {
        get
        {
            if (CurrentHealth <= 0)
                return true;
            else
                return false;
        }
        private set { isDead = value; }
    }
    public UnityEvent onDeath;
    // Use this for initialization
    private void Start()
    {
        currentHealth = ActorStats.Endurance * 10;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            TakeDamage(10);
        }
    }
    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;
        if (IsDead)
        {
            onDeath.Invoke();
        }
    }
}

