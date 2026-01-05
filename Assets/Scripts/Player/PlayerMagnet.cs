using UnityEngine;

/// <summary>
/// 플레이어 자석 기능
/// 플레이어 주위의 드랍 아이템을 끌어당김
/// </summary>
public class PlayerMagnet : MonoBehaviour
{
    private const float CHECK_INTERVAL = 0.1f; // 아이템 감지 간격

    [SerializeField] private LayerMask _itemLayerMask;

    #region 레퍼런스
    private Player _player;
    private DropItemManager _dropItemManager;
    #endregion

    #region 자석 변수
    private float _magnetRadius = 0f;
    private float _pickupRadiusSqr = 0f;
    private bool _isChecking = false;
    private float _nextCheckTime = 0f;
    #endregion

    public void Init(Player player)
    {
        //플레이어 저장
        _player = player;

        //드랍 아이템 매니저 저장
        _dropItemManager = _player.GameManager.DropItemManager;

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

    private void Update()
    {
        HandleCheckItem();
    }

    #region 아이템 체크 및 획득
    // 아이템 체크 처리
    private void HandleCheckItem()
    {
        //체크 중이 아니면 return
        if (!_isChecking) return;

        //체크 시간이 아니면 return
        if (Time.time < _nextCheckTime) return;

        //체크 시간에 현재 시간 + interval 설정
        _nextCheckTime = Time.time + CHECK_INTERVAL;

        //드랍 아이템 매니저에 아이템 체크 요청
        _dropItemManager.CheckDropItems(_player, _magnetRadius, _pickupRadiusSqr);
    }

    private void StartCheckItem()
    {
        _isChecking = true;
    }

    private void StopCheckItem()
    {
        _isChecking = false;
    }
    #endregion
}