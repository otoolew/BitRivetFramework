using Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>{
    public GMData GMData;
    public string titleScene;
    public string startingScene;
    public string currentScene;
    public enum GameState
    {
        STARTMENU,
        RUNNING,
        PAUSED,
        SCENECHANGE,
        GAMEOVER
    }
    public SceneController sceneController;

    public EventGameState OnGameStateChanged;
    
    GameState _currentGameState = GameState.RUNNING;

    string _currentLevelName;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GMData.CurrentGameState = _currentGameState;
        sceneController.OnSceneChangeComplete.AddListener(HandleSceneChangeStart);
        sceneController.OnSceneChangeComplete.AddListener(HandleSceneChangeComplete);
        OnGameStateChanged.Invoke(GMData.CurrentGameState, _currentGameState);
    }

    void Update()
    {
        if (_currentGameState == GameState.SCENECHANGE)
        {
            return;
        }
    }

    public void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;
        GMData.CurrentGameState = _currentGameState;
        switch (CurrentGameState)
        {
            case GameState.SCENECHANGE:
                // Initialize any systems that need to be reset
                Debug.Log("Scene Changing");
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                //  Unlock player, enemies and input in other systems, update tick if you are managing time
                Debug.Log("Game Running");
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                // Pause player, enemies etc, Lock other input in other systems
                Debug.Log("Game Paused");
                Time.timeScale = 0f;
                break;
            case GameState.GAMEOVER:
                // Pause player, enemies etc, Lock other input in other systems
                Debug.Log("Game OVER");
                Time.timeScale = 1.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }
    public void StartGame()
    {
        sceneController.FadeAndLoadScene(sceneController.Scenes[1].sceneName);
    }
    public void RestartLevel()
    {
        sceneController.FadeAndLoadScene(sceneController.CurrentScene);
    }
    public void QuitToTitle()
    {
        sceneController.FadeAndLoadScene(sceneController.Scenes[0].sceneName);
    }
    public void QuitGame()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif      
    }

    public void HandleSceneChangeStart(bool started)
    {
        Debug.Log("[GameManager] Scene Change Start.");
        UpdateState(GameState.SCENECHANGE);
    }

    public void HandleSceneChangeComplete(bool complete)
    {
        Debug.Log("[GameManager] Scene Change Complete.");
        UpdateState(GameState.RUNNING);
    }
    [System.Serializable] public class EventGameState : UnityEvent<GameState, GameState> { }
}
