using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 드롭 아이템 매니저
/// 드롭 아이템 오브젝트 풀링 및 스폰 기능 담당
/// </summary>
public class DropItemManager : MonoBehaviour
{
    private Dictionary<DropItemData, ObjectPool<DropItem>> _dropItemPools = new();

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
        SpawnDropItems(enemy.EnemyData.DropTable, enemy.transform.position);
    }

    private void OnAnyReleaseRequested(DropItem item)
    {
        var pool = GetPool(item.DropItemData);
        pool.Release(item);
    }
    #endregion

    #region 오브젝트 풀링
    private void InitPool(DropItemData dropItemData)
    {
        ObjectPool<DropItem> pool = new(
            () =>
            {
                var item = Instantiate(dropItemData.ItemPrefab);
                item.Init(dropItemData);
                item.gameObject.SetActive(false);
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
            }
        }
    }
    #endregion
}