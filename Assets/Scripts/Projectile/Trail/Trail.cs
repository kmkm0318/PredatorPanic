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
    public TrailRenderer TrailRenderer { get; private set; }
    #endregion

    //컴포넌트 가져오기
    private void Awake()
    {
        TrailRenderer = GetComponent<TrailRenderer>();
    }

    //트레일 데이터 초기화
    public void Init(TrailData trailData)
    {
        TrailData = trailData;
    }
}