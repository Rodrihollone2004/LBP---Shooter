using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private GameObject enemyPrefab;

    public void RespawnEnemy(Vector3 position)
    {
        StartCoroutine(RespawnCoroutine(position));
    }

    private IEnumerator RespawnCoroutine(Vector3 position)
    {
        yield return new WaitForSeconds(respawnDelay);

        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
