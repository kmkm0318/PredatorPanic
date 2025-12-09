using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 적 매니저 클래스
/// 적 스폰 및 관리 기능 담당
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _spawnRange = 20f;
    [SerializeField] private float _checkStartHeight = 5f;
    [SerializeField] private int _maxTryCount = 10;
    [SerializeField] private LayerMask _groundLayer;

    #region 레퍼런스
    private GameManager _gameManager;
    #endregion

    #region 오브젝트 풀
    private Dictionary<EnemyData, ObjectPool<Enemy>> _enemyPools = new();
    private List<Enemy> _activeEnemies = new(); //현재 활성화된 적 리스트
    private List<Enemy> _activeBossEnemies = new(); //현재 활성화된 보스 적 리스트
    #endregion

    #region 적 스폰 변수
    private bool _isSpawning = false;
    private Transform _target;
    private int _spawnCount;
    private float _spawnInterval;
    private int _spawnLevel;
    private float _nextSpawnTime = 0f;
    private EnemyTableData _currentEnemyTable;
    public bool IsBossRound { get; private set; } = false;
    private bool _isBossSpawned = false;
    #endregion

    #region 이벤트
    public event Action OnAllBossDead;
    #endregion

    public void Init(GameManager gamemanager)
    {
        _gameManager = gamemanager;
        _target = _gameManager.Player.transform;
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void Update()
    {
        HandleEnemySpawn();
    }

    #region 이벤트
    private void RegisterEvents()
    {
        Enemy.OnAnyBossDeath += OnAnyBossDeath;
        Enemy.OnAnyReleaseRequested += ReleaseEnemy;
    }

    private void UnregisterEvents()
    {
        Enemy.OnAnyBossDeath += OnAnyBossDeath;
        Enemy.OnAnyReleaseRequested -= ReleaseEnemy;
    }

    //아무 보스나 사망했을 때
    private void OnAnyBossDeath(Enemy enemy)
    {
        //활성화된 보스 리스트에서 제거
        _activeBossEnemies.Remove(enemy);

        //보스가 모두 사망 시 이벤트 호출
        if (_activeBossEnemies.Count == 0)
        {
            OnAllBossDead?.Invoke();
        }
    }

    //적 해제 요청 시
    private void ReleaseEnemy(Enemy enemy)
    {
        //풀이 존재할 시 반환
        if (_enemyPools.TryGetValue(enemy.EnemyData, out var pool))
        {
            pool.Release(enemy);
            _activeEnemies.Remove(enemy);
        }
        //풀이 존재하지 않을 시 파괴(오류 방지)
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

    #region Enemy 스폰 제어
    private void HandleEnemySpawn()
    {
        //스폰 중이 아니면 return
        if (!_isSpawning) return;

        //스폰 시간이 아니면 return
        if (Time.time < _nextSpawnTime) return;

        //적 스폰
        SpawnEnemies();

        //스폰 시간 갱신
        _nextSpawnTime = Time.time + _spawnInterval;
    }

    //적 스폰 변수 설정
    public void SetRoundEnemyVariables()
    {
        //게임 데이터 가져오기
        var gameData = _gameManager.GameData;

        //라운드 인덱스는 현재 라운드 -1
        int roundIdx = _gameManager.CurrentRound - 1;

        //적 테이블 설정
        var enemyTableIdx = Mathf.Min(roundIdx, gameData.EnemyTableListData.EnemyTableDatas.Count - 1);
        _currentEnemyTable = gameData.EnemyTableListData.EnemyTableDatas[enemyTableIdx];

        //보스 라운드 설정
        IsBossRound = _currentEnemyTable.BossEnemyDatas == null || _currentEnemyTable.BossEnemyDatas.Count > 0;
        _isBossSpawned = false;

        //스폰 수 계산
        _spawnCount = gameData.BaseEnemySpawnCount
        + roundIdx
        * gameData.EnemySpawnCountIncrementPerRound;

        //스폰 속도 계산
        float enemySpawnSpeed = gameData.BaseEnemySpawnSpeed
        + roundIdx
        * gameData.EnemySpawnSpeedIncrementPerRound;

        //스폰 간격 계산
        _spawnInterval = 1f / enemySpawnSpeed;

        //스폰 레벨은 라운드 인덱스와 동일
        _spawnLevel = roundIdx;

        //다음 스폰 시간 초기화
        _nextSpawnTime = 0f;
    }

    public void StartEnemySpawn()
    {
        _isSpawning = true;

        //보스 라운드인 경우 보스 스폰
        if (IsBossRound && !_isBossSpawned)
        {
            SpawnBossEnemy();
        }
    }

    public void StopEnemySpawn()
    {
        _isSpawning = false;
    }
    #endregion

    #region Enemy 스폰
    //일반 적 여러마리 소환
    private void SpawnEnemies()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            EnemyData enemyData = _currentEnemyTable.EnemyDatas.GetRandomItem();
            if (enemyData == null)
            {
                Debug.LogError("EnemyManager: EnemyData is null.");
                continue;
            }

            if (!TryGetRandomSpawnPoint(_target, out var spawnPoint))
            {
                Debug.LogWarning("EnemyManager: Failed to find valid spawn point.");
                continue;
            }

            Enemy enemy = SpawnEnemy(enemyData, spawnPoint);
            enemy.SetTarget(_target);

            _activeEnemies.Add(enemy);
        }
    }

    //보스 적 소환
    private void SpawnBossEnemy()
    {
        // 보스 라운드가 아니거나 이미 소환한 경우 return
        if (!IsBossRound || _isBossSpawned) return;
        _isBossSpawned = true;

        var bossList = _currentEnemyTable.BossEnemyDatas;

        foreach (var bossData in bossList)
        {
            if (!TryGetRandomSpawnPoint(_target, out var spawnPoint))
            {
                Debug.LogWarning("EnemyManager: Failed to find valid spawn point for boss.");
                continue;
            }

            Enemy bossEnemy = SpawnEnemy(bossData, spawnPoint);
            bossEnemy.SetTarget(_target);

            _activeEnemies.Add(bossEnemy);
            _activeBossEnemies.Add(bossEnemy);
        }
    }

    //단일 적 스폰
    private Enemy SpawnEnemy(EnemyData enemyData, Vector3 spawnPoint)
    {
        //풀 가져오고 적 가져오기
        var pool = GetPool(enemyData);
        var enemy = pool.Get();

        //적 초기화
        enemy.Init(enemyData, _spawnLevel);
        enemy.transform.position = spawnPoint;
        return enemy;
    }

    //랜덤 스폰 위치 계산
    private bool TryGetRandomSpawnPoint(Transform target, out Vector3 pos)
    {
        //트라이 횟수만큼 시도
        for (int i = 0; i < _maxTryCount; i++)
        {
            //타겟 주변의 랜덤 위치 계산
            var randomOffsetXY = UnityEngine.Random.insideUnitCircle.normalized * _spawnRange;
            var randomOffsetXZ = new Vector3(randomOffsetXY.x, 0f, randomOffsetXY.y);
            var randomPos = target.position + randomOffsetXZ;

            //레이를 통해 지면을 확인
            var rayStartPos = randomPos + Vector3.up * _checkStartHeight;
            if (Physics.Raycast(rayStartPos, Vector3.down, out var hit, float.MaxValue, _groundLayer))
            {
                pos = hit.point;
                return true;
            }
        }

        //유효한 위치를 찾지 못함
        pos = Vector3.zero;
        return false;
    }
    #endregion

    #region 적 일괄 처리
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
    #endregion
}