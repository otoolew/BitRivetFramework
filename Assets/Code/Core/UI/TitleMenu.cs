// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
	public class TitleMenu : MonoBehaviour 
	{
        TitleUIController uiController;
	
		// Update is called once per frame
		void Update () 
		{
            //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
            if (Input.GetButtonDown("Cancel"))
            {
                uiController.DisplayQuitConfirmMenu();
            }
        }

    }
}