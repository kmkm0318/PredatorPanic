using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 드롭 아이템 매니저
/// 드롭 아이템 오브젝트 풀링 및 스폰 기능 담당
/// </summary>
public class DropItemManager : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<DropItemData, ObjectPool<DropItem>> _dropItemPools = new();
    private List<DropItem> _activeDropItems = new(); //현재 활성화된 드롭 아이템 리스트
    #endregion

    #region 레퍼런스
    private GameManager _gameManager;
    #endregion

    #region 변수
    public bool CanDrop { get; set; } = true;
    #endregion

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
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
        Enemy.OnAnyDeath += OnAnyEnemyDeath;
        Enemy.OnAnyBossDeath += OnAnyBossEnemyDeath;
        DropItem.OnAnyReleaseRequested += OnAnyReleaseRequested;
    }

    private void UnregisterEvents()
    {
        Enemy.OnAnyDeath -= OnAnyEnemyDeath;
        Enemy.OnAnyBossDeath -= OnAnyBossEnemyDeath;
        DropItem.OnAnyReleaseRequested -= OnAnyReleaseRequested;
    }

    private void OnAnyEnemyDeath(Enemy enemy)
    {
        //드랍 불가 시 패스
        if (!CanDrop) return;

        //보스 라운드일 시 패스
        //보스의 드랍 아이템은 따로 처리
        if (_gameManager.EnemyManager.IsBossRound) return;

        //아이템 드랍
        SpawnDropItems(enemy.EnemyData.DropTable, enemy.transform.position);
    }

    private void OnAnyBossEnemyDeath(Enemy enemy)
    {
        //드랍 불가 시 패스
        if (!CanDrop) return;

        //아이템 드랍
        SpawnDropItems(enemy.EnemyData.DropTable, enemy.transform.position);
    }

    private void OnAnyReleaseRequested(DropItem item)
    {
        var pool = GetPool(item.DropItemData);
        pool.Release(item);
        _activeDropItems.Remove(item);
    }
    #endregion

    #region 오브젝트 풀링
    private void InitPool(DropItemData dropItemData)
    {
        ObjectPool<DropItem> pool = new(
            () =>
            {
                var item = Instantiate(dropItemData.ItemPrefab, transform);
                item.Init(dropItemData);
                return item;
            },
            (item) => item.gameObject.SetActive(true),
            (item) => item.gameObject.SetActive(false),
            (item) => Destroy(item.gameObject),
            false
        );

        _dropItemPools[dropItemData] = pool;
    }

    private ObjectPool<DropItem> GetPool(DropItemData dropItemData)
    {
        if (!_dropItemPools.TryGetValue(dropItemData, out var pool))
        {
            InitPool(dropItemData);
            pool = _dropItemPools[dropItemData];
        }

        return pool;
    }
    #endregion

    #region 아이템 스폰
    public void SpawnDropItems(DropTableData dropTable, Vector3 position)
    {
        //드랍 테이블의 각 아이템에 대해 각각 진행
        foreach (var dropItemData in dropTable.DropItems)
        {
            //랜덤 값이 드랍 확률 이상일 시 패스
            if (Random.value >= dropItemData.DropRate) continue;

            //풀 가져오기
            var pool = GetPool(dropItemData);

            //드랍 개수 결정
            int dropCount = Random.Range(dropItemData.MinDropCount, dropItemData.MaxDropCount + 1);

            //드랍 개수만큼 드랍
            for (int i = 0; i < dropCount; i++)
            {
                //아이템 가져오기
                var dropItem = pool.Get();

                //랜덤 오프셋 계산
                var randomOffset = Random.insideUnitCircle * dropItemData.DropRadius;
                var randomOffsetXZ = new Vector3(randomOffset.x, 0f, randomOffset.y);

                //아이템 위치 설정
                dropItem.transform.position = position + randomOffsetXZ;

                //아이템 초기화
                dropItem.ResetItem();

                //활성화된 드롭 아이템 리스트에 추가
                _activeDropItems.Add(dropItem);
            }
        }
    }
    #endregion

    #region 필드 위 드랍 아이템
    public void CollectAllDropItems(Player player)
    {
        //뒤에서부터 접근해서 문제 방지
        for (int i = _activeDropItems.Count - 1; i >= 0; i--)
        {
            var item = _activeDropItems[i];
            item.StartFollowPlayer(player);
        }
    }

    //필드 위에 드랍 아이템이 있는지 확인
    public bool HasActiveDropItems()
    {
        return _activeDropItems.Count > 0;
    }
    #endregion
}