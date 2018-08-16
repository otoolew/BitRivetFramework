using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {
    public Button StartGameButton;
    public Button QuitButton;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(HandleResumeClick);
        QuitButton.onClick.AddListener(HandleQuitClick);
    }

    void HandleResumeClick()
    {
        GameManager.Instance.StartGame();
    }

    void HandleQuitClick()
    {
        GameManager.Instance.QuitGame();
    }
}
