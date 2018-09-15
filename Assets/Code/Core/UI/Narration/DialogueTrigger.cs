using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    DialogueController _dialogueController;

    [SerializeField]
    public PlayerController PlayerController { get; private set; }

    [SerializeField]
    private  float distanceToPlayer;
    public float DistanceToPlayer
    { get { return Vector3.Distance(transform.position, PlayerController.PlayerPosition);}}
    public float speakingRange;
    public Dialogue dialogue;
    public bool playerInput;

    private void Start()
    {
        PlayerController = FindObjectOfType<PlayerController>();
        _dialogueController = FindObjectOfType<DialogueController>();
    }
    private void Update()
    {
        distanceToPlayer = Vector3.Distance(gameObject.transform.position, PlayerController.transform.position);
        playerInput = PlayerController.playerInput.Interact;
        if (PlayerController.playerInput.Interact)
        {
            Debug.Log("Time to Talk");
        }
    }
    public void TriggerDialogue()
    {
        _dialogueController.StartDialogue(dialogue);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            TriggerDialogue();      
        
    }
    void OnMouseOver()
    {
        if ((DistanceToPlayer <= speakingRange))
        {
            TriggerDialogue();
        }
    }

}
