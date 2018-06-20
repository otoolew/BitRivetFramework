// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    20 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
namespace TicTacToe
{
    public class GameController : MonoBehaviour
    {

        public Text[] buttonList;

        private string playerSide;

        void Awake()
        {
            SetGameControllerReferenceOnButtons();
            playerSide = "X";
        }

        void SetGameControllerReferenceOnButtons()
        {
            for (int i = 0; i < buttonList.Length; i++)
            {
                buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);

            }
        }

        public string GetPlayerSide()
        {
            return playerSide;
        }

        public void EndTurn()
        {
            if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver();
            }
        }

        void GameOver()
        {
            for (int i = 0; i < buttonList.Length; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
        }
    }
}