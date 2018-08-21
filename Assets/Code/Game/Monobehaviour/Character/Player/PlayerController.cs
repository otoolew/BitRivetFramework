using Core.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the central manager that will manage stats between systems
/// </summary>
public class PlayerController : ActorController
{
    #region Components
    private Animator animator;
    #endregion

    #region Fields / Properties
    [SerializeField]
    readonly Vector3 playerPosition;
    public Vector3 PlayerPosition
    { get { return transform.position; } }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<HealthController>().TakeDamage(10);
        }
    }

    public override void HandleDeath()
    {
        GameManager.Instance.UpdateState(GameManager.GameState.GAMEOVER);
    }
}
