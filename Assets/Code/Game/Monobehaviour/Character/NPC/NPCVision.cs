using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    float timer = 0;
    Ray ray;
    RaycastHit rayHit;
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
    private Transform currentTarget;
    public Transform CurrentTarget
    {
        get { return currentTarget; }
        set { currentTarget = value; }
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

        if (timer >= DetectionRate)
        {
            timer = 0f;
            FindVisableTargets();
        }
    }
    private void FindVisableTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
        foreach (var item in targetsInView)
        {
            Transform target = item.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2)
            {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, out rayHit, targetDistance))
                {
                    //Debug.Log("Debug RayHit: " + rayHit.collider.name);
                    if (rayHit.collider.transform.tag.Equals(DetectionTag))
                        visibleTargets.Add(rayHit.collider.transform);
                }
            }

        }
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
