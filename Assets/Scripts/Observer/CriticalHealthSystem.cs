using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CriticalHealthSystem : MonoBehaviour, IHealthObserver
{
    [SerializeField] private Image imageCritical;
    [SerializeField] private PlayerHealth health;

    private void Awake()
    {
        health.OnHealthChanged += OnHealthChanged;
    }
    public void OnHealthChanged(int newHealth)
    {
        imageCritical.enabled = newHealth < 20 ? true : false;
    }
}
