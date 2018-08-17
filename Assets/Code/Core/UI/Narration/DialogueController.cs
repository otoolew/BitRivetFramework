using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialoguePanel dialoguePanel;
    private Queue<string> sentences;

	// Use this for initialization
	void Start ()
    {      
        sentences = new Queue<string>();
	}

    public Events.EventStartDialogue OnStartDialogue;
    public Events.EventFinishDialogue OnFinishDialogue;

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting Dialogue with " + dialogue.speakerName);
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.imageSprite.sprite = dialogue.speakerImage;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialoguePanel.dialogueText.text = sentence;
        Debug.Log(sentence);
    }
    public void EndDialogue()
    {
        dialoguePanel.gameObject.SetActive(false);
        Debug.Log("End of Conversation");
    }
}
