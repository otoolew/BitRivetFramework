using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanel : MonoBehaviour
{
    public Text speakerName;
    public Image imageSprite;
    public Text dialogueText;
    public Button ContinueButton;

    private void Awake()
    {
        ContinueButton.onClick.AddListener(HandleContinueClick);
    }

    void HandleContinueClick()
    {
        FindObjectOfType<DialogueController>().DisplayNextSentence();
    }

}

