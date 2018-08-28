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
    private readonly float distanceToPlayer;
    public float DistanceToPlayer
    { get { return Vector3.Distance(transform.position, PlayerController.PlayerPosition);}}
    public float speakingRange;
    public Dialogue dialogue;


    private void Start()
    {
        PlayerController = FindObjectOfType<PlayerController>();
        _dialogueController = FindObjectOfType<DialogueController>();
    }
    public void TriggerDialogue()
    {
        _dialogueController.StartDialogue(dialogue);
    }

    void OnMouseOver()
    {
        if ((DistanceToPlayer <= speakingRange))
        {
            TriggerDialogue();
        }
    }

}
