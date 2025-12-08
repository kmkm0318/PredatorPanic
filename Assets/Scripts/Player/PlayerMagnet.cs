using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어 자석 기능
/// 플레이어 주위의 드랍 아이템을 끌어당김
/// </summary>
public class PlayerMagnet : MonoBehaviour
{
    private const float CHECK_INTERVAL = 0.1f; // 아이템 감지 간격

    [SerializeField] private LayerMask _itemLayerMask;

    private Player _player;

    #region 자석 변수
    private float _magnetRadius = 0f;
    private float _pickupRadiusSqr = 0f;
    #endregion

    #region 코루틴
    private Coroutine _checkItemCoroutine;
    #endregion

    public void Init(Player player)
    {
        //플레이어 저장
        _player = player;

        //자석 반경 스탯 변경 시 콜백 등록
        player.PlayerStats.GetStat(PlayerStatType.MagnetRadius).OnValueChanged += SetMagnetRadius;

        //초기 자석 반경 설정
        SetMagnetRadius(player.PlayerStats.GetStat(PlayerStatType.MagnetRadius).FinalValue);

        //아이템 픽업 반경 설정
        _pickupRadiusSqr = player.PlayerData.ItemPickupRadiusSqr;
    }

    private void SetMagnetRadius(float value)
    {
        _magnetRadius = value;
    }

    private void OnEnable()
    {
        StartCheckItem();
    }

    private void OnDisable()
    {
        StopCheckItem();
    }

    #region 체크 아이템 코루틴
    private IEnumerator CheckItemCoroutine()
    {
        //대기 타이머
        var wait = new WaitForSeconds(CHECK_INTERVAL);

        while (true)
        {
            //자석 반경 내의 아이템 감지
            int count = PhysicsUtility.GetOverlapSphereNonAlloc(transform.position, _magnetRadius, _itemLayerMask, out var colliders);

            //플레이어 위치
            Vector3 playerPos = transform.position;

            //아이템 수만큼 반복
            for (int i = 0; i < count; i++)
            {
                if (colliders[i] == null) continue;

                //드랍 아이템이 아닐 시 패스
                if (!colliders[i].TryGetComponent<DropItem>(out var dropItem)) continue;

                //아이템이 플레이어를 따라오도록 설정되어 있을 시
                if (dropItem.DropItemData.IsFollow)
                {
                    //아직 따라오고 있지 않을 시 따라오게 함
                    if (!dropItem.IsFollowing)
                    {
                        dropItem.StartFollowPlayer(_player);
                    }
                }
                //아이템이 플레이어를 따라오는 게 아닐 시
                else
                {
                    //픽업 반경 내에 있을 시 즉시 픽업
                    var distanceSqr = (dropItem.transform.position - playerPos).sqrMagnitude;
                    if (distanceSqr <= _pickupRadiusSqr)
                    {
                        dropItem.OnPickup(_player);
                    }
                }
            }

            yield return wait;
        }
    }

    private void StartCheckItem()
    {
        StopCheckItem();
        _checkItemCoroutine = StartCoroutine(CheckItemCoroutine());
    }

    private void StopCheckItem()
    {
        if (_checkItemCoroutine != null)
        {
            StopCoroutine(_checkItemCoroutine);
            _checkItemCoroutine = null;
        }
    }
    #endregion
}