using System;
using UnityEngine;

/// <summary>
/// 드롭 아이템 추상 클래스
/// 플레이어가 획득할 수 있는 아이템의 기본 동작 정의
/// </summary>
public abstract class DropItem : MonoBehaviour, IManualUpdate
{
    #region 데이터
    public DropItemData DropItemData { get; private set; }
    #endregion

    #region 따라가기
    private Player _player;
    public bool IsFollowing { get; private set; }
    private float _speed = 0f;
    #endregion

    #region 변수
    public bool IsActive { get; set; } = false;
    #endregion

    #region 이벤트
    public event Action<DropItem> OnPickuped;
    #endregion

    public virtual void Init(DropItemData dropItemData)
    {
        DropItemData = dropItemData;
    }

    public virtual void ResetItem()
    {
        IsFollowing = false;
        _player = null;
        _speed = 0f;
    }

    public void ManualUpdate(float deltaTime)
    {
        //따라가기 처리
        HandleFollow(deltaTime);
    }

    #region 따라가기 및 획득
    public void StartFollowPlayer(Player player)
    {
        //IsFollow가 아니거나 이미 따라가고 있을 시 패스
        if (!DropItemData.IsFollow) return;
        if (IsFollowing) return;

        //플레이어 저장 및 따라가기 시작
        _player = player;
        IsFollowing = true;
        _speed = DropItemData.InitialFollowSpeed;
    }

    private void HandleFollow(float deltaTime)
    {
        //따라가기 중이 아닐 시, 플레이어가 없을 시 패스
        if (!IsFollowing || _player == null) return;

        //방향 계산
        var dirToPlayer = (_player.CenterPosition - transform.position).normalized;

        //속도 갱신
        _speed += DropItemData.FollowSpeedAcceleration * deltaTime;

        //위치 갱신
        transform.position += _speed * deltaTime * dirToPlayer;

        //플레이어에 가까워졌을 시
        var distanceSqr = (_player.CenterPosition - transform.position).sqrMagnitude;
        if (distanceSqr <= _player.PlayerData.ItemPickupRadiusSqr)
        {
            //아이템 획득 처리
            OnPickup(_player);
        }
    }

    //플레이어가 아이템을 획득했을 때 호출
    //각각의 아이템이 이 함수를 오버라이드하여 고유 효과를 구현해야 함.
    //반드시 base.OnPickup(player)를 호출할 것
    public virtual void OnPickup(Player player)
    {
        //따라가기 중지
        IsFollowing = false;

        //효과음 재생
        AudioManager.Instance.PlaySfx(DropItemData.PickupSfxData, transform.position);

        //획득 이벤트 호출
        OnPickuped?.Invoke(this);
    }
    #endregion
}