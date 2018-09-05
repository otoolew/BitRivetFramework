using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class NPCController : ActorController
{
    #region Components
    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
        private set { animator = value; }
    }

    private NPCMovement npcMovement;
    public NPCMovement NPCMovement
    {
        get { return npcMovement; }
        private set { npcMovement = value; }
    }

    private NPCAttack npcAttack;
    public NPCAttack NPCAttack
    {
        get { return npcAttack; }
        private set { npcAttack = value; }
    }
    private NPCVision npcVision;
    public NPCVision NPCVision
    {
        get { return npcVision; }
        private set { npcVision = value; }
    }

    private PlayerController playerController;
    public PlayerController PlayerController
    {
        get { return playerController; }
        private set { playerController = value; }
    }


    #endregion

    #region Properties and Variables
    public string NPCName;
    public float CorpseLingerTime;
    public float AlertTime;
    public float SearchTime;

    [SerializeField]
    private readonly float distanceToPlayer;
    public float DistanceToPlayer
    {
        get { return Vector3.Distance(transform.position, PlayerController.PlayerPosition); }
    }

    private DamageZone[] HitColliders;

    public bool Dead;

    #endregion
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        npcMovement = GetComponent<NPCMovement>();
        npcAttack = GetComponent<NPCAttack>();
        npcVision = GetComponent<NPCVision>();
        HitColliders = GetComponentsInChildren<DamageZone>();
        PlayerController = FindObjectOfType<PlayerController>();
        ActivateHitColliders();           
    }

    private void ActivateHitColliders()
    {
        foreach (var collider in HitColliders)
        {
            collider.GetComponent<Collider>().enabled = true;
        }
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        npcMovement = GetComponent<NPCMovement>();
        npcAttack = GetComponent<NPCAttack>();
        npcVision = GetComponent<NPCVision>();

        NPCMovement.ActivateNavAgent();
        npcVision.enabled = true;
        HitColliders = GetComponentsInChildren<DamageZone>();
        PlayerController = FindObjectOfType<PlayerController>();
        ActivateHitColliders();
        Dead = false;

    }
    private void OnDisable()
    {
    
    }
    private void Update()
    {
        
        animator.SetBool("HasTarget", npcVision.HasTarget);
        if (npcVision.HasTarget)
        {
            animator.SetFloat("PlayerDistance", Vector3.Distance(gameObject.transform.position, playerController.PlayerPosition));
            
        }
            
    }
    public void LookAtTarget(GameObject target)
    {
        // Create a vector from the npc to the target.
        Vector3 rotVector = target.transform.position - transform.position;

        // Ensure the vector is entirely along the floor plane.
        rotVector.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotation = Quaternion.LookRotation(rotVector);

        // Set the character's rotation to this new rotation.
        transform.rotation = newRotation;
    }
    public override void HandleDeath()
    {
        animator.SetBool("IsDead", true);
        Dead = true;
        //npcMovement.NavAgent.enabled = false;
        StartCoroutine("DecaySequence");
        Debug.Log(gameObject.name + " is dead.");
    }
    IEnumerator DecaySequence()
    {
        InvokeRepeating("DeathDecay", CorpseLingerTime, 0.01f);
        yield return new WaitForSeconds(CorpseLingerTime + 3);
        CancelInvoke();
        gameObject.SetActive(false);
    }
    public void DeathDecay()
    {
        transform.Translate(-Vector3.up * .5f * Time.deltaTime);
    }

}


