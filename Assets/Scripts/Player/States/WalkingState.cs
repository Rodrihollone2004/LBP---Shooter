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
        player.CalculateVertical();

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move(moveDirection, player.WalkSpeed);

        if (player.IsGrounded)
        {
            player.VerticalVelocity = 0;

            if (player.playerInput.IsRunning)
            {
                player.TransitionToState(new RunningState());
            }
        }
        else if (player.playerInput.IsJumping)
        {
            player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);
            player.TransitionToState(new JumpingState());
        }
        else if (player.playerInput.IsCrouch)
        {
            player.TransitionToState(new CrouchingState());
        }
    }

    public void ExitState(PlayerController player)
    {
    }
}


