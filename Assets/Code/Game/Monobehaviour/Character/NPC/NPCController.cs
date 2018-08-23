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
    private PlayerController playerController;
    public PlayerController PlayerController
    {
        get { return playerController; }
        private set { playerController = value; }
    }
    AimIK aimIK;
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

    [SerializeField]
    private NPCTask currentRunningTask;

    [SerializeField]
    private DamageZone[] HitColliders;

    public Transform aimPoint;
    #endregion
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        aimIK = GetComponent<AimIK>();
        npcMovement = GetComponent<NPCMovement>();
        HitColliders = GetComponentsInChildren<DamageZone>();
        PlayerController = FindObjectOfType<PlayerController>();
        InitTaskStack();
        ActivateHitColliders();
    }
    private void InitTaskStack()
    {
        _taskStack = new Stack<NPCTask>();
        _taskStack.Push(taskList[0]);
        currentRunningTask = _taskStack.Peek();
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
        NPCMovement.ActivateNavAgent();

        HitColliders = GetComponentsInChildren<DamageZone>();
        PlayerController = FindObjectOfType<PlayerController>();

        InitTaskStack();
        ActivateHitColliders();

    }
    private void OnDisable()
    {
        _taskStack.Clear();
    }
    private void Update()
    {
          
        if (Input.GetKeyUp(KeyCode.Z))
        {
            AddTask(taskList[0]);
            currentRunningTask = _taskStack.Peek();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            AddTask(taskList[1]);
            currentRunningTask = _taskStack.Peek();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            AddTask(taskList[2]);
            currentRunningTask = _taskStack.Peek();
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            CompleteCurrentTask();
        }
        RunTask();

    }
    public void RunTask()
    {
        if (_taskStack.Count < 1)
        {
            _taskStack.Push(taskList[0]);
        }
        try
        {
            currentRunningTask = _taskStack.Peek();
        }
        catch (System.NullReferenceException)
        {
            _taskStack.Push(taskList[0]);
            currentRunningTask = _taskStack.Peek();
        }
        currentRunningTask.PerformTask(this);
    }
    public void Idle()
    {
        NPCMovement.Stop();
    }
    public void Patrol()
    {
        NPCMovement.ContinuePatrol();
    }
    public void Attack()
    {
        aimIK.enabled = true;
        aimPoint.transform.position = PlayerController.PlayerPosition + new Vector3(0,2,0);

        //Debug.Log("Player is "+DistanceToPlayer+ " far away.");
        if (DistanceToPlayer > 5f)
        {
            NPCMovement.GoToPosition(PlayerController.PlayerPosition);
        }
        else
        {
            NPCMovement.Stop();
            Vector3 enemyToplayer = aimPoint.position - transform.position;
            enemyToplayer.y = 0;

            if (enemyToplayer != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyToplayer.normalized), 0.15f);
            }
            else
            {
                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(enemyToplayer);

                // Set the player's rotation to this new rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
            }
        }
            
    }
    public void AddTask(NPCTask task)
    {
        if(!_taskStack.Contains(task))
            _taskStack.Push(task);
    }
    public void CompleteCurrentTask()
    {
        if (_taskStack.Count < 1)
            return;
        _taskStack.Pop(); 
    }
    public override void HandleDeath()
    {
        animator.SetBool("IsDead", true);
        NPCMovement.Stop();
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


