using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private GameObject enemyPrefab;

    private Transform enemiesContainer;

    private void Awake()
    {
        enemiesContainer = GameObject.Find("EnemysContainer")?.transform;
    }
    public void RespawnEnemy(Vector3 position)
    {
        StartCoroutine(RespawnCoroutine(position));
    }

    private IEnumerator RespawnCoroutine(Vector3 position)
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        if (enemiesContainer != null)
        {
            newEnemy.transform.SetParent(enemiesContainer);
        }
    }
}
