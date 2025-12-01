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
    [Header("Enemy Settings")]
    [SerializeField] private List<EnemyData> _enemyDataList;
    [SerializeField] private int _maxTryCount = 10;

    #region 적 소환 Planet
    private Planet _planet;
    #endregion

    #region 오브젝트 풀
    private Dictionary<EnemyData, ObjectPool<Enemy>> _enemyPools = new();
    private List<Enemy> _activeEnemies = new(); //현재 활성화된 적 리스트
    #endregion

    #region 코루틴
    private Coroutine _enemySpawnCoroutine;
    #endregion

    public void Init(Planet planet)
    {
        _planet = planet;
    }

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
            () => Instantiate(enemyData.EnemyPrefab, transform),
            (enemy) => enemy.gameObject.SetActive(true),
            (enemy) => enemy.gameObject.SetActive(false),
            (enemy) => Destroy(enemy.gameObject)
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

    //단일 적 스폰
    public Enemy SpawnEnemy(EnemyData enemyData, Vector3 spawnPoint)
    {
        //풀 가져오고 적 가져오기
        var pool = GetPool(enemyData);
        var enemy = pool.Get();

        //적 초기화
        enemy.Init(enemyData, 0, _planet);
        enemy.transform.position = spawnPoint;
        return enemy;
    }

    //랜덤 스폰 위치 계산
    private bool TryGetRandomSpawnPoint(Transform target, out Vector3 pos)
    {
        //트라이 횟수만큼 시도
        for (int i = 0; i < _maxTryCount; i++)
        {
            //행성 표면 랜덤 위치 계산
            var randomPos = _planet.Center + _planet.Radius * Random.onUnitSphere;

            //타겟과의 거리 계산
            var dist = Vector3.Distance(randomPos, target.position);

            //타겟과 충분히 떨어져 있는지 확인
            if (dist > _planet.Radius)
            {
                pos = randomPos;
                return true;
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