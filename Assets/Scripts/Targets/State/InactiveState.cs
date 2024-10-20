using UnityEngine;
public class InactiveState : ITargetState
{
    public void Activate(ShootingRange shootingRange, int index)
    {
        shootingRange.Targets[index].GetComponent<Renderer>().material.color = shootingRange.ActiveColor;
        shootingRange.SetState(new ActiveState()); 
        shootingRange.IsTargetActive = true; 
    }

    public void Deactivate(ShootingRange shootingRange, int index)
    {
        // Ya esta inactivo, no es necesario volver a desactivarlo
    }
}
