using System.Collections;

public interface IGrenade
{
    IEnumerator GrenadeController();

    void Throw();

    void ResetThrow();

    void GrenadeInputs();
}