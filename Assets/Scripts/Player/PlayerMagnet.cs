using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어 자석 기능
/// 플레이어 주위의 드랍 아이템을 끌어당김
/// </summary>
public class PlayerMagnet : MonoBehaviour
{
    private const int MAX_CHECK_NUM = 256; // 한번에 최대 256개의 아이템 감지
    private const float CHECK_INTERVAL = 0.1f; // 아이템 감지 간격

    [SerializeField] private LayerMask _itemLayerMask;

    private Player _player;

    #region 자석 변수
    private float _magnetRadius = 0f;
    private Collider[] _hitColliders = new Collider[MAX_CHECK_NUM];
    #endregion

    #region 코루틴
    private Coroutine _checkItemCoroutine;
    #endregion

    public void Init(Player player)
    {
        _player = player;
        player.PlayerStats.GetStat(PlayerStatType.MagnetRadius).OnValueChanged += SetMagnetRadius;
        SetMagnetRadius(player.PlayerStats.GetStat(PlayerStatType.MagnetRadius).FinalValue);
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
        WaitForSeconds wait = new(CHECK_INTERVAL);

        while (true)
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _magnetRadius, _hitColliders, _itemLayerMask);
            for (int i = 0; i < count; i++)
            {
                if (_hitColliders[i] == null) continue;

                if (!_hitColliders[i].TryGetComponent<DropItem>(out var dropItem)) continue;

                if (!dropItem.DropItemData.IsFollow || dropItem.IsFollowing) continue;

                dropItem.StartFollowPlayer(_player);
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