using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 매니저 클래스
/// 적 스폰 및 관리 기능 담당
/// </summary>
[RequireComponent(typeof(EnemySpawner))]
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    [SerializeField] private float _spawnRadius = 20f;
    [SerializeField] private float _spawnHeightMax = 20f;
    [SerializeField] private int _maxTryCount = 10;
    [SerializeField] private LayerMask _groundLayerMask;

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
                continue;
            }

            if (!TryGetRandomSpawnPoint(target, out var spawnPoint))
            {
                Debug.LogWarning("EnemyManager: Failed to find valid spawn point.");
                continue;
            }

            Enemy enemy = _enemySpawner.SpawnEnemy(enemyData, spawnPoint);
            enemy.SetTarget(target);
        }
    }

    //랜덤 스폰 포인트 계산. NavMesh 위의 유효한 위치 반환
    private bool TryGetRandomSpawnPoint(Transform target, out Vector3 pos)
    {
        for (int i = 0; i < _maxTryCount; i++)
        {
            Vector2 randomPosXZ = Random.insideUnitCircle.normalized * _spawnRadius;
            Vector3 rayOrigin = target.position + new Vector3(randomPosXZ.x, _spawnHeightMax, randomPosXZ.y);
            Ray ray = new(rayOrigin, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _spawnHeightMax * 2f, _groundLayerMask))
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
}