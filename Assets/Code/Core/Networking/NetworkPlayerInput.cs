using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerInput : MonoBehaviour {

    protected Vector2 m_Movement;
    public Vector2 MoveInput
    {
        get { return m_Movement;}
    }

    public KeyCode clickKey;
    protected bool m_Click;
    public bool Click
    {
        get { return m_Click; }
    }

    public KeyCode attackKey;
    protected bool m_Attack;
    public bool Attack
    {
        get { return m_Attack; }
    }

    public KeyCode aimKey;
    protected bool m_Aim;
    public bool AimInput
    {
        get { return m_Aim; }
    }

    public KeyCode crouchKey;
    protected bool m_Crouch;
    public bool CrouchInput
    {
        get { return m_Crouch; }
    }

    public KeyCode interactKey;
    protected bool m_Interact;
    public bool Interact
    {
        get { return m_Interact; }
    }
    private void Start()
    {
        
    }
    void Update()
    {

        m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_Crouch = Input.GetKey(crouchKey);
        m_Aim = Input.GetKey(aimKey);
        m_Interact = Input.GetKeyDown(interactKey);
        m_Click = Input.GetKeyDown(clickKey);

    }
}
