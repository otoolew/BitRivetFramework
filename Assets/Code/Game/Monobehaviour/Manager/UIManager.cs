using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private PauseMenu _pauseMenu;

    private void Start()
    {       
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void Update()
    {

    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        switch (currentState)
        {
            case GameManager.GameState.RUNNING:
                _pauseMenu.gameObject.SetActive(false);
                break;
            case GameManager.GameState.PAUSED:    
                _pauseMenu.gameObject.SetActive(true);
                break;
            default:
                _pauseMenu.gameObject.SetActive(false);
                break;
        }
    }

}
