public interface ITargetState
{
    void Activate(ShootingRange shootingRange, int index);
    void Deactivate(ShootingRange shootingRange, int index);
}
