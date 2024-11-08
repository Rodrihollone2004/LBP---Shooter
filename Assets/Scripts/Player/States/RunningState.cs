using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : IState
{
    public void EnterState(PlayerController player)
    {
    }

    public void UpdateState(PlayerController player)
    {

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move();

        if (Input.GetKeyDown(KeyCode.E) && player.DashCooldownTimer <= 0)
        {
            player.TransitionToState(new DashingState());
        }

        if (player.IsGrounded())
        {
            player.VerticalVelocity = 0;

            if (!player.playerInput.IsRunning)
            {
                player.TransitionToState(new WalkingState());
            }
        }
        else if (player.playerInput.IsJumping)
        {
            player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);
            player.TransitionToState(new JumpingState());
        }
    }

    public void ExitState(PlayerController player)
    {
    }
}


