using Core;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    float timer;
    public PlayerController playerController;
    public Transform eyes;

    Ray ray;
    RaycastHit rayHit;
    AimIK aimIK;

    [SerializeField]
    private float viewRadius;
    public float ViewRadius
    {
        get { return viewRadius; }
        set { viewRadius = value; }
    }
    [SerializeField]
    private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
        set { attackRadius = value; }
    }
    [SerializeField]
    [Range(0, 360)]
    private float viewAngle;
    public float ViewAngle
    {
        get { return viewAngle; }
        set { viewAngle = value; }
    }

    [SerializeField]
    private bool detectionRange;
    public bool DetectionRange
    {
        get { return detectionRange; }
        set { detectionRange = value; }
    }
    [SerializeField]
    private bool attackRange;
    public bool AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
    [SerializeField]
    private bool hasTarget;
    public bool HasTarget
    {
        get { return hasTarget; }
        set { hasTarget = value; }
    }
    public LayerMask targetLayer;

    [SerializeField]
    private List<Transform> visibleTargets;
    public List<Transform> VisibleTargets
    {
        get { return visibleTargets; }
        set { visibleTargets = value; }
    }

    [SerializeField]
    private string detectionTag;
    public string DetectionTag
    {
        get { return detectionTag; }
        set { detectionTag = value; }
    }

    [SerializeField]
    private float playerDistance;
    public float PlayerDistance
    {
        get { return playerDistance; }
        set { playerDistance = value; }
    }
    
    public RayCastLine rayLine;
    public float FireRate;
    public Transform aimPoint;
    public Transform CurrentTarget;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        aimIK = GetComponent<AimIK>();
    }
    // Use this for initialization
    void Start()
    {
        ray.origin = transform.position;
        visibleTargets = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        playerDistance = Vector3.Distance(transform.position, playerController.PlayerPosition);
        detectionRange = PlayerDistance < viewRadius;
        attackRange = PlayerDistance < attackRadius;
        if (DetectionRange)
        {
            FindVisableTargets();
            // Add the time since Update was last called to the timer.
        }
    }
    private void FindVisableTargets()
    {
        visibleTargets.Clear();
        DamageZone[] playerTargets = playerController.DamageColliders;

        foreach (var item in playerTargets)
        {
            Transform target = item.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2)
            {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, out rayHit, targetDistance))
                {
                    if (rayHit.collider.transform.tag.Equals(DetectionTag))
                        visibleTargets.Add(rayHit.collider.transform);
                }
            }
        }
        if(visibleTargets.Count > 0)
        {
            CurrentTarget = FindObjectOfType<DamageZone>().transform;           
        }
    }
    
    public void Fire()
    {
        if (timer >= FireRate)
        {
            timer = 0;
            rayLine.Fire();
        }
    }

}
