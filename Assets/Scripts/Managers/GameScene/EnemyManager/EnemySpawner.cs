using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 적 스포너 클래스
/// 적 오브젝트 풀링 및 스폰 기능 담당
/// </summary>
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
            () =>
            {
                Enemy enemy = Instantiate(enemyData.EnemyPrefab);
                enemy.OnRelease += (e) => ReleaseEnemy(enemyData, e);
                enemy.gameObject.SetActive(false);
                return enemy;
            },
            (enemy) =>
            {
                enemy.gameObject.SetActive(true);
                enemy.Init(enemyData);
            },
            (enemy) => enemy.gameObject.SetActive(false),
            (enemy) => Destroy(enemy.gameObject),
            false,
            10,
            1000
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