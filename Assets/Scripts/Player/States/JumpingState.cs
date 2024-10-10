using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Entrando al estado: Saltar");
        player.CalculateVertical();
    }

    public void UpdateState(PlayerController player)
    {
        player.CalculateVertical();

        Vector3 inputVector = player.playerInput.InputVector;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.z).normalized;
        player.Move(moveDirection, player.WalkSpeed);

        if (player.IsGrounded)
        {
            player.TransitionToState(new WalkingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        Debug.Log("Saliendo del estado: Saltar");
    }
}
