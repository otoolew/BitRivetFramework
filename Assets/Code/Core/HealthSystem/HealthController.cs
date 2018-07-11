using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth;
        /// <summary>
        /// The max health of this instance
        /// </summary>
        public float MaxHealth
        {
            get { return maxHealth; }
            set{ maxHealth = value; }
        }

        [SerializeField]
        private float startingHealth;
        /// <summary>
        /// The starting health of this instance
        /// </summary>
        public float StartingHealth
        {
            get { return startingHealth; }
            set { startingHealth = value; }
        }

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
        [SerializeField]
        private bool isDead;
        /// <summary>
        /// Is the intance dead
        /// </summary>
        public bool IsDead
        {
            get { return CurrentHealth <= 0; }
            private set { isDead = value; }
        }

        public KeyCode killKey;

        // Use this for initialization
        private void Start()
        {

        }
        private void OnEnable()
        {
            currentHealth = startingHealth;
        }
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(killKey))
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public void TakeDamage(float damageValue)
        {
            currentHealth -= damageValue;
        }
    }
}
