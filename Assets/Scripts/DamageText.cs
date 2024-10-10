using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 randomiseOffset;
    [SerializeField] private Color damageColour;

    private TextMeshProUGUI damageText;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        transform.localPosition += offset;
        transform.localPosition += new Vector3(
            Random.Range(-randomiseOffset.x, randomiseOffset.x),
            Random.Range(-randomiseOffset.y, randomiseOffset.y),
            Random.Range(-randomiseOffset.z, randomiseOffset.z)
        );
        Destroy(gameObject, destroyTime);
    }

    public void Initialise(int damageValue)
    {
        damageText.text = damageValue.ToString();
    }
}
