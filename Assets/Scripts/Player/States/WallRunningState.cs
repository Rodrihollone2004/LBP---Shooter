using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunningState : IState
{
    public void EnterState(PlayerController player)
    {
        player.IsWallRunning = true;
        player.VerticalVelocity = 0f; 
    }

    public void UpdateState(PlayerController player)
    {
        if (player.IsTouchingWall())
        {
            Vector3 moveDirection = Vector3.Cross(player.wallHit.normal, Vector3.up);
            player.Move(moveDirection, player.RunSpeed);

            player.VerticalVelocity += player.Gravity * 0.1f * Time.deltaTime;

            if (player.playerInput.IsJumping)
            {
                player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);
                player.TransitionToState(new JumpingState());
            }
            else if (!player.playerInput.IsRunning)
            {
                player.TransitionToState(new WalkingState());
            }
        }
        else
        {
            player.TransitionToState(new WalkingState());
        }
    }


    public void ExitState(PlayerController player)
    {
        player.IsWallRunning = false;
        player.VerticalVelocity = 0f; 
    }
}


