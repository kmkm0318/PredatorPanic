using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 드롭 아이템 추상 클래스
/// 플레이어가 획득할 수 있는 아이템의 기본 동작 정의
/// </summary>
public abstract class DropItem : MonoBehaviour
{
    #region 데이터
    public DropItemData DropItemData { get; private set; }
    #endregion

    #region 따라가기
    private Player _player;
    public bool IsFollowing { get; private set; }
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
        StopAllCoroutines();
        IsFollowing = false;
        _player = null;
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

        // 따라가기 코루틴 시작
        StartCoroutine(FollowCoroutine());
    }

    private IEnumerator FollowCoroutine()
    {
        if (_player == null) yield break;

        //중간 위치 오프셋. 베지어 곡선을 통해 이동하도록.
        var middleOffset = Vector3.up * DropItemData.FollowHeight;

        //베지어 곡선 적용
        //시작 위치
        Vector3 p0 = transform.position;

        //중간 위치
        //플레이어의 반대 방향으로 거리만큼 + offset만큼 올림
        Vector3 p1 = p0 + (p0 - _player.CenterPosition) + middleOffset;

        //스피드에 따라 이동 시간 계산
        float speed = DropItemData.FollowSpeed;
        float duration = Vector3.Distance(p0, _player.CenterPosition) / speed;

        //이동 시간 동안 베지어 곡선을 따라 이동
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float u = Mathf.Clamp01(elapsed / duration);
            float v = 1 - u;

            //베지어 곡선 공식
            Vector3 newPos = v * v * p0 + 2 * v * u * p1 + u * u * _player.CenterPosition;
            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        //마지막 위치 지정
        transform.position = _player.CenterPosition;
        OnPickup(_player);
    }

    //플레이어가 아이템을 획득했을 때 호출
    //각각의 아이템이 이 함수를 오버라이드하여 고유 효과를 구현해야 함.
    //반드시 base.OnPickup(player)를 호출할 것
    public virtual void OnPickup(Player player)
    {
        IsFollowing = false;
        OnPickuped?.Invoke(this);
    }
    #endregion
}