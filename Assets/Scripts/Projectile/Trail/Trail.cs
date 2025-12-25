using System;
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
    private Transform _targetTransform;
    #endregion

    #region 라이프타임
    private bool _isLifetimeRunning = false;
    private float _lifetimeElapsed = 0f;
    private float _lifetimeDuration = 0f;
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

    private void Update()
    {
        HandleLifetime();
    }

    private void LateUpdate()
    {
        HandleMovement();
    }

    #region 총알 관련
    private void HandleMovement()
    {
        //비활성화 시 패스
        if (!gameObject.activeSelf) return;

        //타겟이 없으면 패스
        if (_targetTransform == null) return;

        //트레일 위치 갱신
        transform.position = _targetTransform.position;
    }

    public void AttachToBullet(Bullet bullet)
    {
        //타겟으로 설정
        _targetTransform = bullet.transform;

        //위치 초기화
        transform.position = _targetTransform.position;

        //트레일 초기화
        _trailRenderer.Clear();
        _trailRenderer.emitting = true;
    }

    public void DetachFromBullet()
    {
        //타겟 해제
        _targetTransform = null;

        //트레일 방출 중지
        _trailRenderer.emitting = false;

        //일정 시간 후 트레일 반환
        StartLifetime(_trailRenderer.time);
    }
    #endregion

    #region 라이프 타임
    private void HandleLifetime()
    {
        // 비활성화 시 패스
        if (!gameObject.activeSelf) return;

        //라이프타임이 동작 중이지 않으면 패스
        if (!_isLifetimeRunning) return;

        //경과 시간 갱신
        _lifetimeElapsed += Time.deltaTime;

        //지속 시간 경과 시
        if (_lifetimeElapsed >= _lifetimeDuration)
        {
            //비활성화
            StopLifetime();
        }
    }

    private void StartLifetime(float duration)
    {
        //기존 라이프타임 중지
        StopLifetime();

        //변수 설정
        _lifetimeDuration = duration;
        _lifetimeElapsed = 0f;
        _isLifetimeRunning = true;
    }

    private void StopLifetime()
    {
        //라이프 타임이 동작 중이지 않으면 패스
        if (!_isLifetimeRunning) return;

        //변수 초기화
        _isLifetimeRunning = false;

        //트레일 반환
        _trailManager.ReleaseTrail(this);
    }
    #endregion
}