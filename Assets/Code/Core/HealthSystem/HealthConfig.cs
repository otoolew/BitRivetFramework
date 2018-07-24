using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [CreateAssetMenu(fileName = "newHPConfig", menuName ="Health System/Health Configuration")]
    public class HealthConfig : ScriptableObject
    {
        [Range(1, 100)]
        public float MaxHealth;

    }
}
