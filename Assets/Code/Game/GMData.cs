using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameManagerData", menuName = "Game Manager Data")]
public class GMData : ScriptableObject
{
    public GameManager.GameState _currentGameState;
    
    public GameManager.GameState CurrentGameState
    {
        get { return _currentGameState; }
        set { _currentGameState = value; }
    }

}
