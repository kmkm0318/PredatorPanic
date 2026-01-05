using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 드롭 아이템 매니저
/// 드롭 아이템 오브젝트 풀링 및 스폰 기능 담당
/// 매니저의 위치를 y -1000 등으로 설정해서 버그 예방
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
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트
    private void RegisterEvents()
    {
        var enemyManager = _gameManager.EnemyManager;

        if (enemyManager)
        {
            enemyManager.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    private void UnregisterEvents()
    {
        var enemyManager = _gameManager.EnemyManager;

        if (enemyManager)
        {
            enemyManager.OnEnemyDeath -= HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        //드랍 불가 시 패스
        if (!CanDrop) return;

        //보스 라운드이면서 보스가 아닐 시 패스
        if (_gameManager.EnemyManager.IsBossRound && !enemy.EnemyData.IsBoss) return;

        //아이템 드랍
        SpawnDropItems(enemy.EnemyData.DropItemTable, enemy.transform.position);
    }
    #endregion

    private void Update()
    {
        ManualUpdateDropItem();
    }

    private void ManualUpdateDropItem()
    {
        //시간 캐싱
        float deltaTime = Time.deltaTime;

        //뒤에서부터 접근해서 문제 방지
        for (int i = _activeDropItems.Count - 1; i >= 0; i--)
        {
            var item = _activeDropItems[i];

            //Active 상태일 시 수동 업데이트
            if (item.IsActive)
            {
                item.ManualUpdate(deltaTime);
            }

            //Active 상태가 아니거나 업데이트 후 비활성화된 아이템 처리
            if (!item.IsActive)
            {
                //맨 뒤 아이템으로 교체
                int lastIndex = _activeDropItems.Count - 1;
                _activeDropItems[i] = _activeDropItems[lastIndex];

                //목록에서 제거
                _activeDropItems.RemoveAt(lastIndex);
            }
        }
    }

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
            (item) =>
            {
                //비활성화
                item.gameObject.SetActive(false);

                //위치 초기화
                item.transform.position = transform.position;
            },
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
    public void SpawnDropItems(DropItemTableData dropItemTable, Vector3 position)
    {
        //y 위치 보정
        position.y = 0f;

        //드랍 테이블의 각 아이템에 대해 각각 진행
        foreach (var entry in dropItemTable.DropItemTableEntries)
        {
            //확률 검사 실패 시 패스
            if (!entry.DropRate.ChanceTest()) continue;

            //드랍 아이템 데이터 가져오기
            var dropItemData = entry.DropItemData;

            //풀 가져오기
            var pool = GetPool(dropItemData);

            //드랍 개수 결정
            int dropCount = Random.Range(entry.MinCount, entry.MaxCount + 1);

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

                //아이템 활성화
                dropItem.IsActive = true;

                //아이템 이벤트 등록
                dropItem.OnPickuped += HandlePickuped;

                //활성화된 드롭 아이템 리스트에 추가
                _activeDropItems.Add(dropItem);
            }
        }
    }

    //드롭 아이템 획득 시
    private void HandlePickuped(DropItem item)
    {
        //이미 비활성화된 아이템일 시 패스
        if (!item.IsActive) return;

        //비활성화
        item.IsActive = false;

        //이벤트 해제
        item.OnPickuped -= HandlePickuped;

        //아이템 반환
        var pool = GetPool(item.DropItemData);
        pool.Release(item);
    }
    #endregion

    #region 필드 위 드랍 아이템
    public void CheckDropItems(Player player, float magnetRadius, float pickupRadiusSqr)
    {
        //플레이어 기준 위치
        var playerPos = player.transform.position;

        //뒤에서부터 접근해서 문제 방지
        for (int i = _activeDropItems.Count - 1; i >= 0; i--)
        {
            var dropItem = _activeDropItems[i];
            var dropItemPos = dropItem.transform.position;

            //자석 반경 밖일 시 패스
            if ((dropItemPos - playerPos).sqrMagnitude > magnetRadius * magnetRadius) continue;

            //아이템이 플레이어를 따라오도록 설정되어 있을 시
            if (dropItem.DropItemData.IsFollow)
            {
                //아직 따라오고 있지 않을 시 따라오게 함
                if (!dropItem.IsFollowing)
                {
                    dropItem.StartFollowPlayer(player);
                }
            }
            //아이템이 플레이어를 따라오는 게 아닐 시
            else
            {
                //픽업 반경 내에 있을 시 즉시 픽업
                var distanceSqr = (dropItem.transform.position - playerPos).sqrMagnitude;
                if (distanceSqr <= pickupRadiusSqr)
                {
                    dropItem.OnPickup(player);
                }
            }
        }
    }

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