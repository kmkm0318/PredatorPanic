using System.Collections;
using UnityEngine;

/// <summary>
/// 트레일 클래스
/// 총알 등에 붙어서 궤적을 남김
/// </summary>
[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    #region 데이터
    public TrailData TrailData { get; private set; }
    #endregion

    #region 컴포넌트
    private TrailRenderer _trailRenderer;
    #endregion

    #region 레퍼런스
    private TrailManager _trailManager;
    #endregion

    //컴포넌트 가져오기
    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    //트레일 데이터 초기화
    public void Init(TrailData trailData, TrailManager trailManager)
    {
        TrailData = trailData;
        _trailManager = trailManager;
    }

    //불릿에 부착
    public void AttachToBullet(Bullet bullet)
    {
        //부모로 설정 및 위치, 방향 초기화
        transform.SetParent(bullet.transform, false);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        //트레일 초기화
        _trailRenderer.Clear();
        _trailRenderer.emitting = true;
    }

    //불릿에서 분리
    public void DetachFromBullet()
    {
        //부모를 트레일 매니저로 변경
        transform.SetParent(_trailManager.transform, true);

        //트레일 방출 중지
        _trailRenderer.emitting = false;

        //일정 시간 후 트레일 반환
        StartCoroutine(DelayReleaseCoroutine());
    }

    private IEnumerator DelayReleaseCoroutine()
    {
        yield return new WaitForSeconds(_trailRenderer.time);

        _trailManager.ReleaseTrail(this);
    }
}