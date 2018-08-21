using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
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
    private PlayerController playerController;
    public PlayerController PlayerController
    {
        get { return playerController; }
        private set { playerController = value; }
    }
    public PatrolCircuit patrolCircuit;
    #endregion
    #region Properties and Variables
    public float CorpseLingerTime;

    [SerializeField]
    private readonly float distanceToPlayer;
    public float DistanceToPlayer
    {
        get
        {
            return Vector3.Distance(transform.position, PlayerController.PlayerPosition);
        }

    }
    private Stack<NPCTask> _taskStack;
    public List<NPCTask> taskList;
    #endregion
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        npcMovement = GetComponent<NPCMovement>();
        PlayerController = FindObjectOfType<PlayerController>();
        _taskStack = new Stack<NPCTask>();
        Idle();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Patrol();
        }
    }
    public void Idle()
    {
        _taskStack.Push(taskList[0]);
        Debug.Log(gameObject.name + " task is [Idle].");
    }
    public void Patrol()
    {
        _taskStack.Push(taskList[1]);
        Debug.Log(gameObject.name + " task is [Patrol]");
        NPCMovement.TargetPoint = patrolCircuit.wayPoints[0];
    }
    public void Attack()
    {
        Debug.Log(gameObject.name + " task is [Attack]");
        _taskStack.Push(taskList[2]);
    }

    public override void HandleDeath()
    {
        animator.SetBool("IsDead", true);
        StartCoroutine("DecaySequence");
        Debug.Log(gameObject.name + " is dead.");
    }
    IEnumerator DecaySequence()
    {
        InvokeRepeating("DeathDecay", CorpseLingerTime, 0.01f);
        yield return new WaitForSeconds(CorpseLingerTime + 4);
        CancelInvoke();
    }
    public void DeathDecay()
    {
        transform.Translate(-Vector3.up * .5f * Time.deltaTime);
    }
}


