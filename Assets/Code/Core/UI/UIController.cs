// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    27 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;

namespace Core
{
	public class UIController : MonoBehaviour 
	{
        #region Fields / Properties

        [SerializeField]
        private bool alwaysDisplayMouse;
        public bool AlwaysDisplayMouse
        {
            get
            {
                return alwaysDisplayMouse;
            }

            set
            {
                alwaysDisplayMouse = value;
            }
        }

        [SerializeField]
        private GameObject pauseCanvas;
        public GameObject PauseCanvas
        {
            get
            {
                return pauseCanvas;
            }

            set
            {
                pauseCanvas = value;
            }
        }

        [SerializeField]
        private GameObject optionsCanvas;
        public GameObject OptionsCanvas
        {
            get
            {
                return optionsCanvas;
            }

            set
            {
                optionsCanvas = value;
            }
        }

        [SerializeField]
        private GameObject controlsCanvas;
        public GameObject ControlsCanvas
        {
            get
            {
                return controlsCanvas;
            }

            set
            {
                controlsCanvas = value;
            }
        }

        [SerializeField]
        private GameObject audioCanvas;
        public GameObject AudioCanvas
        {
            get
            {
                return audioCanvas;
            }

            set
            {
                audioCanvas = value;
            }
        }

        [SerializeField]
        private bool inPause;
        protected bool InPause
        {
            get
            {
                return inPause;
            }

            set
            {
                inPause = value;
            }
        }
        #endregion

  
        public void ExitPause()
        {
            InPause = true;
            SwitchPauseState();
        }

        public void RestartLevel()
        {
            InPause = true;
            SwitchPauseState();

        }

        void Update()
        {
            if (PlayerInput.Instance != null && PlayerInput.Instance.Pause)
            {
                SwitchPauseState();
            }
        }

        protected void SwitchPauseState()
        {
            if (InPause && Time.timeScale > 0 || !InPause && ScreenFader.IsFading)
                return;

            if (InPause)
                PlayerInput.Instance.GainControl();
            else
                PlayerInput.Instance.ReleaseControl();

            Time.timeScale = InPause ? 1 : 0;

            if (PauseCanvas)
                PauseCanvas.SetActive(!InPause);

            if (OptionsCanvas)
                OptionsCanvas.SetActive(false);

            if (ControlsCanvas)
                ControlsCanvas.SetActive(false);

            if (AudioCanvas)
                AudioCanvas.SetActive(false);

            InPause = !InPause;
        }
    }
}