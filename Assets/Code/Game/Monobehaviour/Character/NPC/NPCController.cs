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

    public float stateTimeElapsed;
    public State currentState;
    public State remainState;
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
        if(!Dead)
            currentState.UpdateState(this);
    }
    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
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


