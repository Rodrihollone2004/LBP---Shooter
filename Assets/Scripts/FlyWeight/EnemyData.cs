using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "FlyWeight/Enemy Data", order = 1)]

public class EnemyData : ScriptableObject
{
    [SerializeField] public Color maxHealthColor = Color.green;
    [SerializeField] public Color zeroHealthColor = Color.red;
    [SerializeField] public GameObject damageTextPrefab;
}
