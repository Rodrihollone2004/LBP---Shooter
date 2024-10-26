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
            Vector3 wallRight = Vector3.Cross(Vector3.up, player.wallHit.normal);  
            Vector3 wallLeft = -wallRight;

            Vector3 moveDirection = Vector3.Dot(player.transform.forward, wallRight) > 0 ? wallRight : wallLeft;

            player.Move(moveDirection, player.RunSpeed);

            player.VerticalVelocity += player.Gravity * 0.1f * Time.deltaTime;

            if (player.playerInput.IsJumping)
            {
                player.JumpOffWall();
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


