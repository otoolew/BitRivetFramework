using Core.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newActorStats", menuName = "Actor/Stats")]
public class ActorStats : ScriptableObject
{
    public string UnitType;
    public float Strength;
    public float Agility;
    public float Endurance;
    public float Charisma;
    public List<StringVariable> Alliances;
}
