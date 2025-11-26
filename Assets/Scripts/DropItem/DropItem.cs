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
    public static event Action<DropItem> OnAnyReleaseRequested;
    #endregion

    public virtual void Init(DropItemData dropItemData)
    {
        DropItemData = dropItemData;
        IsFollowing = false;
    }

    public void StartFollowPlayer(Player player)
    {
        if (!DropItemData.IsFollow) return;
        if (IsFollowing) return;

        _player = player;
        IsFollowing = true;

        StartCoroutine(FollowCoroutine());
    }

    private IEnumerator FollowCoroutine()
    {
        if (_player == null) yield break;

        //플레이어 Transform과 오프셋
        var playerTransform = _player.transform;
        var startOffset = Vector3.up * DropItemData.FollowHeight;
        var endOffset = Vector3.up * DropItemData.TargetHeight;

        //베지어 곡선 적용
        Vector3 p0 = transform.position;
        Vector3 p1 = 2 * p0 - playerTransform.position + startOffset;
        float duration = DropItemData.FollowDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float u = Mathf.Clamp01(elapsed / duration);
            float v = 1 - u;

            //베지어 곡선 공식
            Vector3 newPos = v * v * p0 + 2 * v * u * p1 + u * u * (playerTransform.position + endOffset);
            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = playerTransform.position + endOffset;
        OnPickup(_player);
    }

    public virtual void OnPickup(Player player)
    {
        IsFollowing = false;
        OnAnyReleaseRequested?.Invoke(this);
    }
}