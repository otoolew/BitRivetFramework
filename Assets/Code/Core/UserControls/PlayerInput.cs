using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class PlayerInput : Singleton<PlayerInput> 
	{
        //[HideInInspector]
        public bool playerControllerInputBlocked;
        protected bool externalInputBlocked;

        public KeyCode pauseKey;
        protected bool m_Pause;

        protected Vector2 m_Movement;
        public Vector2 MoveInput
        {
            get
            {
                if (playerControllerInputBlocked || externalInputBlocked)
                    return Vector2.zero;
                return m_Movement;
            }
        }

        public KeyCode clickKey;
        protected bool m_Click;
        public bool Click
        {
            get { return m_Click && !playerControllerInputBlocked && !externalInputBlocked; }
        }



        public KeyCode attackKey;
        protected bool m_Attack;
        public bool Attack
        {
            get { return m_Attack && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public KeyCode aimKey;
        protected bool m_Aim;
        public bool AimInput
        {
            get { return Input.GetKey(aimKey) && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public KeyCode crouchKey;
        protected bool m_Crouch;
        public bool CrouchInput
        {
            get { return m_Crouch && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public KeyCode interactKey;
        protected bool m_Interact;
        public bool Interact
        {
            get { return m_Interact && !playerControllerInputBlocked && !externalInputBlocked; }
        }
        private void Start()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        }
        void Update()
        {
            if (Input.GetKeyUp(pauseKey)){
                GameManager.Instance.TogglePause();
            }
            m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_Crouch = Input.GetKey(crouchKey);
            m_Aim = Input.GetKey(aimKey);
            m_Interact = Input.GetKeyDown(interactKey);
            m_Click = Input.GetKeyDown(clickKey);

        }

        public bool HaveControl()
        {
            return !externalInputBlocked;
        }

        public void FreezeControl()
        {
            externalInputBlocked = true;
        }

        public void GainControl()
        {
            externalInputBlocked = false;
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            switch (currentState)
            {
                case GameManager.GameState.PAUSED:
                    FreezeControl();
                    break;
                case GameManager.GameState.RUNNING:
                    GainControl();
                    break;
                default:
                    Debug.Log("PlayerInput: Default Case Hit");
                    FreezeControl();
                    break;
            }
        }

    }
}
