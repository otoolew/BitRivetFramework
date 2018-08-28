using Core;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    float timer;
    public PlayerController playerController;

    Ray ray;
    RaycastHit rayHit;


    [SerializeField]
    private bool inAttackRange;
    public bool InAttackRange
    {
        get { return inAttackRange; }
        set { inAttackRange = value; }
    }
    [SerializeField]
    private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }


    [SerializeField]
    private List<DamageZone> targetZones;
    public List<DamageZone> TargetZones
    {
        get { return targetZones; }
        set { targetZones = value; }
    }

    [SerializeField]
    private float playerDistance;
    public float PlayerDistance
    {
        get { return playerDistance; }
        set { playerDistance = value; }
    }
    
    public RayCastLine rayLine;
    public float fireRate;
    public Transform targetCursor;
    public DamageZone currentTarget;
    public LayerMask targetLayer;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    // Use this for initialization
    void Start()
    {
        ray.origin = transform.position;
        targetZones = new List<DamageZone>();
        RefreshTargets();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        playerDistance = Vector3.Distance(transform.position, playerController.PlayerPosition);
        inAttackRange = PlayerDistance < attackRange;        
    }
    public void RefreshTargets()
    {
        targetZones.Clear();
        foreach (var damageZone in playerController.DamageColliders)
        {
            Transform target = damageZone.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToTarget, out rayHit, playerDistance, targetLayer))
            {
                DamageZone temp = rayHit.transform.GetComponent<DamageZone>();
                if (currentTarget == null)
                {
                    currentTarget = temp;
                    return;
                }
                if(temp.Priority < currentTarget.Priority)
                {
                    currentTarget = temp;
                }      
            }           
        }
    }
    public void LegRotation()
    {
        // Create a vector from the npc to the target.
        Vector3 rotVector = playerController.transform.position - transform.position;

        // Ensure the vector is entirely along the floor plane.
        rotVector.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotation = Quaternion.LookRotation(rotVector);

        // Set the character's rotation to this new rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
       
    }
    public void Fire()
    {
        RefreshTargets();
        if (timer >= fireRate)
        {
            timer = 0;
            rayLine.Fire();
        }
    }

}
