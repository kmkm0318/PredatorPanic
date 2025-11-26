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

    #region 변수
    public bool CanDrop { get; set; } = true;
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
        Enemy.OnAnyDeath += OnEnemyDeath;
        DropItem.OnAnyReleaseRequested += OnAnyReleaseRequested;
    }

    private void UnregisterEvents()
    {
        Enemy.OnAnyDeath -= OnEnemyDeath;
        DropItem.OnAnyReleaseRequested -= OnAnyReleaseRequested;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if (CanDrop)
        {
            SpawnDropItems(enemy.EnemyData.DropTable, enemy.transform.position);
        }
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
            () => Instantiate(dropItemData.ItemPrefab, transform),
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
        foreach (var dropItemData in dropTable.DropItems)
        {
            if (Random.value > dropItemData.DropRate) continue;
            var pool = GetPool(dropItemData);
            int dropCount = Random.Range(dropItemData.MinDropCount, dropItemData.MaxDropCount + 1);

            for (int i = 0; i < dropCount; i++)
            {
                var dropItem = pool.Get();
                var randomOffset = Random.insideUnitCircle * dropItemData.DropRadius;
                dropItem.transform.position = position + new Vector3(randomOffset.x, 0f, randomOffset.y);
                dropItem.Init(dropItemData);
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