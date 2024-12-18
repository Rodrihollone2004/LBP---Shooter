using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : IState
{
    public void EnterState(PlayerController player)
    {
    }

    public void UpdateState(PlayerController player)
    {

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        float speed = player.playerInput.IsRunning ? player.RunSpeed : player.WalkSpeed;
        player.Move();

        if (player.IsGrounded())
        {
            player.TransitionToState(new WalkingState());
        }
        if (Input.GetKeyDown(KeyCode.E) && player.DashCooldownTimer <= 0)
        {
            player.TransitionToState(new DashingState());
        }
    }

    public void ExitState(PlayerController player)
    {
    }
}

