using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

/// <summary>
/// 적 매니저 클래스
/// 적 스폰 및 관리 기능 담당
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    [SerializeField] private float _spawnRadius = 20f;
    [SerializeField] private float _spawnHeightMax = 10f;
    [SerializeField] private int _maxTryCount = 10;
    [SerializeField] private LayerMask _groundLayerMask;

    #region 오브젝트 풀
    private Dictionary<EnemyData, ObjectPool<Enemy>> _enemyPools = new();
    private List<Enemy> _activeEnemies = new(); //현재 활성화된 적 리스트
    #endregion

    #region 코루틴
    private Coroutine _enemySpawnCoroutine;
    #endregion

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    #region 이벤트
    private void RegisterEvents()
    {
        Enemy.OnAnyReleaseRequested += ReleaseEnemy;
    }

    private void UnregisterEvents()
    {
        Enemy.OnAnyReleaseRequested -= ReleaseEnemy;
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        if (_enemyPools.TryGetValue(enemy.EnemyData, out var pool))
        {
            pool.Release(enemy);
            _activeEnemies.Remove(enemy);
        }
        else
        {
            Destroy(enemy.gameObject);
        }
    }
    #endregion

    #region 오브젝트 풀링
    private ObjectPool<Enemy> GetPool(EnemyData enemyData)
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
    #endregion

    #region Enemy Spawn 코루틴
    private IEnumerator EnemySpawnCoroutine(Transform target, int count, float interval)
    {
        while (true)
        {
            SpawnEnemies(target, count);
            yield return new WaitForSeconds(interval);
        }
    }

    public void StartEnemySpawn(Transform target, int count, float interval)
    {
        StopEnemySpawn();
        _enemySpawnCoroutine = StartCoroutine(EnemySpawnCoroutine(target, count, interval));
    }

    public void StopEnemySpawn()
    {
        if (_enemySpawnCoroutine != null)
        {
            StopCoroutine(_enemySpawnCoroutine);
            _enemySpawnCoroutine = null;
        }
    }
    #endregion

    #region Enemy 스폰
    public void SpawnEnemies(Transform target, int count)
    {
        for (int i = 0; i < count; i++)
        {
            EnemyData enemyData = _enemyDataList.GetRandomElement();
            if (enemyData == null)
            {
                Debug.LogError("EnemyManager: EnemyData is null.");
                continue;
            }

            if (!TryGetRandomSpawnPoint(target, out var spawnPoint))
            {
                Debug.LogWarning("EnemyManager: Failed to find valid spawn point.");
                continue;
            }

            Enemy enemy = SpawnEnemy(enemyData, spawnPoint);
            enemy.SetTarget(target);

            _activeEnemies.Add(enemy);
        }
    }

    public Enemy SpawnEnemy(EnemyData enemyData, Vector3 spawnPoint)
    {
        var pool = GetPool(enemyData);
        var enemy = pool.Get();
        enemy.transform.position = spawnPoint;
        return enemy;
    }

    //랜덤 스폰 포인트 계산. NavMesh 위의 유효한 위치 반환
    private bool TryGetRandomSpawnPoint(Transform target, out Vector3 pos)
    {
        for (int i = 0; i < _maxTryCount; i++)
        {
            Vector2 randomPosXZ = Random.insideUnitCircle.normalized * _spawnRadius;
            Vector3 rayOrigin = target.position + new Vector3(randomPosXZ.x, _spawnHeightMax, randomPosXZ.y);
            Ray ray = new(rayOrigin, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _groundLayerMask))
            {
                //Enemy Controller가 NavMeshAgent를 사용하기 때문에 NavMesh 위의 위치인지 확인
                if (NavMesh.SamplePosition(hitInfo.point, out var navHit, 1f, NavMesh.AllAreas))
                {
                    pos = navHit.position;
                    return true;
                }
            }
        }

        pos = Vector3.zero;
        return false;
    }
    #endregion

    // 활성화된 모든 적 즉시 사망 처리.
    public void KillAllEnemies()
    {
        //뒤에서부터 접근해서 문제 방지
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            _activeEnemies[i].Die();
        }

        _activeEnemies.Clear();
    }
}