using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CriticalHealthSystem : MonoBehaviour, IHealthObserver
{
    [SerializeField] private Image imageCritical;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private float blinkInterval = 0.5f;

    private Coroutine blinkCoroutine;

    private void Awake()
    {
        health.OnHealthChanged += OnHealthChanged;
    }
    public void OnHealthChanged(int newHealth)
    {
        if (newHealth < 50)
        {
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkImage());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                imageCritical.enabled = false;
            }
        }
    }

    private IEnumerator BlinkImage()
    {
        while (true)
        {
            imageCritical.enabled = !imageCritical.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
