using UnityEngine;

public class ActiveState : ITargetState
{
    public void Activate(ShootingRange shootingRange, int index)
    {
        // Ya esta activo, no es necesario volver a activarlo
    }

    public void Deactivate(ShootingRange shootingRange, int index)
    {
        shootingRange.Targets[index].GetComponent<Renderer>().material.color = shootingRange.InactiveColor;
        shootingRange.SetState(new InactiveState());
        shootingRange.IsTargetActive = false; 
    }
}
