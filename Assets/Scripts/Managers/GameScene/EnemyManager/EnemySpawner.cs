using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private Dictionary<EnemyData, ObjectPool<Enemy>> _enemyPools = new();

    public Enemy SpawnEnemy(EnemyData enemyData, Vector3 spawnPoint)
    {
        var pool = GetObjectPool(enemyData);
        var enemy = pool.Get();
        enemy.transform.position = spawnPoint;
        return enemy;
    }

    private ObjectPool<Enemy> GetObjectPool(EnemyData enemyData)
    {
        if (!_enemyPools.TryGetValue(enemyData, out var pool))
        {
            InitPool(enemyData);
            pool = _enemyPools[enemyData];
        }

        return pool;
    }

    private void InitPool(EnemyData enemyData)
    {
        ObjectPool<Enemy> pool = new(
            createFunc: () =>
            {
                Enemy enemy = Instantiate(enemyData.EnemyPrefab);
                enemy.Init(enemyData);
                enemy.OnRelease += (e) => ReleaseEnemy(enemyData, e);
                enemy.gameObject.SetActive(false);
                return enemy;
            },
            actionOnGet: (enemy) =>
            {
                enemy.gameObject.SetActive(true);
            },
            actionOnRelease: (enemy) =>
            {
                enemy.gameObject.SetActive(false);
            },
            actionOnDestroy: (enemy) =>
            {
                Destroy(enemy.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );

        _enemyPools[enemyData] = pool;
    }

    private void ReleaseEnemy(EnemyData enemyData, Enemy enemy)
    {
        if (_enemyPools.TryGetValue(enemyData, out var pool))
        {
            pool.Release(enemy);
        }
        else
        {
            Destroy(enemy.gameObject);
        }
    }
}