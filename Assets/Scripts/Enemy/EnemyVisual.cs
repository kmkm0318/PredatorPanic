using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 적 비주얼 클래스
/// </summary>
public class EnemyVisual : MonoBehaviour
{
    // 히트 플래시 지속 시간
    private const float HIT_FLASH_DURATION = 0.1f;

    // 머티리얼 리스트
    private Renderer[] _renderers;

    // 머티리얼 프로퍼티 블럭
    private MaterialPropertyBlock _mpb;

    #region 플래시
    static readonly int _FlashID = Shader.PropertyToID("_Flash");
    bool _isFlashing = false;
    float _flashTimer = 0f;
    #endregion

    #region 스폰
    private Tween _spawnTween;
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // 자식 오브젝트의 모든 렌더러에서 메터리얼 수집
        _renderers = GetComponentsInChildren<Renderer>();

        // 머티리얼 프로퍼티 블럭 초기화
        _mpb = new();

        // 플래시 이펙트 초기화
        StopHitFlash();
    }

    private void OnEnable()
    {
        // 플래시 이펙트 초기화
        StopHitFlash();
    }

    private void Update()
    {
        HandleFlash();
    }

    //플래시 처리 함수
    private void HandleFlash()
    {
        if (!_isFlashing) return;

        _flashTimer -= Time.deltaTime;
        if (_flashTimer <= 0f)
        {
            StopHitFlash();
        }
    }

    //플래시를 시작하는 함수
    public void StartHitFlash()
    {
        //플래시 중이 아닌 경우에 플래시 시작
        if (!_isFlashing)
        {
            _isFlashing = true;

            SetFlashValue(1f);
        }

        //타이머 초기화
        _flashTimer = HIT_FLASH_DURATION;
    }

    private void StopHitFlash()
    {
        // 플래시 종료
        _isFlashing = false;

        SetFlashValue(0f);
    }

    /// 플래시 값 설정
    private void SetFlashValue(float value)
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(_FlashID, value);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    public void PlaySpawnAnimation(float spawnVisualOffsetY, float spawnVisualDuration, Ease spawnVisualEase)
    {
        //이전 트윈이 있으면 종료
        _spawnTween?.Kill(false);

        //시작 위치 설정
        Vector3 startPos = transform.localPosition;
        startPos.y -= spawnVisualOffsetY;

        //스폰 애니메이션 실행
        _spawnTween = transform.DOLocalMoveY(0f, spawnVisualDuration)
            .From(startPos)
            .SetEase(spawnVisualEase);
    }
}