using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthObserver
{
    void OnHealthChanged(int newHealth);
}
