// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
	public class TitleUIController : MonoBehaviour 
	{
        public GameObject TitleMenu;
        public GameObject QuitConfirmMenu;

        public void DisplayTitleMenu()
        {
            TitleMenu.SetActive(true);
        }

        public void HideTitleMenu()
        {
            TitleMenu.SetActive(false);
        }

        public void DisplayQuitConfirmMenu()
        {
            QuitConfirmMenu.SetActive(true);
        }

        public void HideQuitConfirmMenu()
        {
            QuitConfirmMenu.SetActive(false);
        }

    }
}