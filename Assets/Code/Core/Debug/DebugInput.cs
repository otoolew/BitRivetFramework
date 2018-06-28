using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class DebugInput : MonoBehaviour
    {
        public static DebugInput Instance
        {
            get { return s_Instance; }
        }

        protected static DebugInput s_Instance;

        [HideInInspector]
        public bool playerControllerInputBlocked;

        protected Vector2 movement;
        protected bool crouch;
        protected bool aim;
        protected bool m_Attack;
        protected bool pause;
        protected bool externalInputBlocked;

        public Vector2 MoveInput
        {
            get
            {
                if (playerControllerInputBlocked || externalInputBlocked)
                    return Vector2.zero;
                return movement;
            }
        }
        public bool CrouchInput
        {
            get { return crouch && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public bool AimInput
        {
            get { return aim && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public bool Attack
        {
            get { return m_Attack && !playerControllerInputBlocked && !externalInputBlocked; }
        }

        public bool Pause
        {
            get { return pause; }
        }

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one DebugInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }


        void Update()
        {
            movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            crouch = Input.GetButton("Crouch");

            if (Input.GetButtonDown("Fire"))
            {

            }
            if (Input.GetButtonDown("Aim"))
            {

            }
            pause = Input.GetButtonDown("Pause");
        }

        public bool HaveControl()
        {
            return !externalInputBlocked;
        }

        public void ReleaseControl()
        {
            externalInputBlocked = true;
        }

        public void GainControl()
        {
            externalInputBlocked = false;
        }
    }
}
