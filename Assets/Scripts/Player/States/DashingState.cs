using UnityEngine;

public class DashingState : IState
{
    private PlayerController player;
    private float dashDuration;

    public void EnterState(PlayerController player)
    {
        this.player = player;

        dashDuration = player.DashDuration;

        player.StartDash();
    }

    public void UpdateState(PlayerController player)
    {
        dashDuration -= Time.deltaTime;

        if (dashDuration <= 0)
        {
            player.TransitionToState(new WalkingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        player.EndDash();
    }
}