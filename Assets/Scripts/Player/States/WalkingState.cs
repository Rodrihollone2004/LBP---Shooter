using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Entrando al estado: Caminar");
    }

    public void UpdateState(PlayerController player)
    {
        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move(moveDirection, player.WalkSpeed);

        if (player.playerInput.IsRunning)
        {
            player.TransitionToState(new RunningState());
        }
        else if (player.playerInput.IsJumping)
        {
            player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);

            player.TransitionToState(new JumpingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        Debug.Log("Saliendo del estado: Caminar");
    }
}
