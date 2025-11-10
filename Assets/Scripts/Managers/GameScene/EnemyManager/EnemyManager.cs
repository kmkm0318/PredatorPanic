using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    [SerializeField] private float _spawnRadius = 20f;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    public void SpawnEnemies(Transform target, int count)
    {
        for (int i = 0; i < count; i++)
        {
            EnemyData enemyData = _enemyDataList.GetRandomElement();
            if (enemyData == null)
            {
                Debug.LogError("EnemyManager: EnemyData is null.");
                return;
            }

            Vector3 spawnPoint = GetRandomSpawnPoint(target);
            Enemy enemy = _enemySpawner.SpawnEnemy(enemyData, spawnPoint);
            enemy.SetTarget(target);
        }
    }

    private Vector3 GetRandomSpawnPoint(Transform target)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnRadius;
        Vector3 spawnPoint = target.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        return spawnPoint;
    }
}