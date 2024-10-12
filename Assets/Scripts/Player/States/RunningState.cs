using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Entrando al estado: Correr");
    }

    public void UpdateState(PlayerController player)
    {
        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move(moveDirection, player.RunSpeed);

        if (!player.playerInput.IsRunning)
        {
            player.TransitionToState(new WalkingState());
        }
        else if (player.playerInput.IsJumping)
        {
            player.TransitionToState(new JumpingState());
        }
    }


    public void ExitState(PlayerController player)
    {
        Debug.Log("Saliendo del estado: Correr");
    }
}
