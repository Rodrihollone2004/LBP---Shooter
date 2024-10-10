using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Entrando al estado: Agacharse");
        player.AdjustCrouchHeight(0.65f);
    }

    public void UpdateState(PlayerController player)
    {

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        player.Move(moveDirection, player.CrouchSpeed);

        if (!player.playerInput.IsCrouch)
        {
            player.TransitionToState(new WalkingState());
        }
        else if (player.playerInput.IsRunning)
        {
            player.TransitionToState(new RunningState());
        }
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Saliendo del estado: Agacharse");
        player.AdjustCrouchHeight(1f);
    }
}
