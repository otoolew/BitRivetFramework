// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: TicTacToe
// Date:    20 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
	public class GridSpace : MonoBehaviour 
	{
        public Button button;
        public Text buttonText;

        private GameController gameController;

        public void SetGameControllerReference(GameController controller)
        {
            gameController = controller;
        }

        public void SetSpace()
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }
    }
}