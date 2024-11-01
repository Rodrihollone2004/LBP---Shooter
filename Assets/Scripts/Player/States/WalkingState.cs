using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : IState
{
    public void EnterState(PlayerController player)
    {
    }

    public void UpdateState(PlayerController player)
    {

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move();

        if (player.IsGrounded())
        {
            player.VerticalVelocity = 0;

            if (player.playerInput.IsRunning)
            {
                player.TransitionToState(new RunningState());
            }
        }
        else if (player.Velocity.y > 0 || player.Velocity.y < 0)
        {
            player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);
            player.TransitionToState(new JumpingState());
        }
        if (player.playerInput.IsCrouch)
        {
            player.TransitionToState(new CrouchingState());
        }
    }

    public void ExitState(PlayerController player)
    {
    }
}


