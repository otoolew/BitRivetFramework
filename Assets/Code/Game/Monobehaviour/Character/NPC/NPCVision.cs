using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCVision : MonoBehaviour
{
    #region Fields / Properties
    public float timer;
    Ray ray;
    RaycastHit rayHit;
    public Transform eyes; 

    [SerializeField]
    private float viewRadius;
    public float ViewRadius
    {
        get { return viewRadius; }
        set { viewRadius = value; }
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
    private float detectionRate;
    public float DetectionRate
    {
        get { return detectionRate; }
        set { detectionRate = value; }
    }

    public LayerMask targetLayer;

    [SerializeField]
    private string detectionTag;
    public string DetectionTag
    {
        get { return detectionTag; }
        set { detectionTag = value; }
    }
    [SerializeField]
    private Transform currentTarget;
    public Transform CurrentTarget
    {
        get { return currentTarget; }
        set { currentTarget = value; }
    }
    public bool HasTarget;
   

    #endregion
    #region Events
    public UnityEvent onGainSight;
    public UnityEvent onLoseSight;

    #endregion

    // Use this for initialization
    void Start()
    {
        ray.origin = transform.position;
        currentTarget = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        HasTarget = TargetInSight();
        if (timer >= detectionRate)
        {
            timer = 0f;
            HasTarget = TargetInSight();
           
        }
        if (HasTarget)
        {
            onGainSight.Invoke();
        }
        else
        {
            onLoseSight.Invoke();
        }
    }

    private bool TargetInSight()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
        foreach (var item in targetsInView)
        {
            Transform target = item.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2)
            {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, out rayHit, targetDistance, targetLayer))
                {
                    //Debug.Log("Debug RayHit: " + rayHit.collider.name);
                    if (rayHit.collider.transform.tag.Equals(DetectionTag))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
