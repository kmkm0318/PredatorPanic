using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이리스 페이드 클래스
/// 아이리스 인, 아웃을 처리합니다
/// </summary>
public class IrisFade : MonoBehaviour
{
    [SerializeField] private Image _irisImage;
    [SerializeField] private Ease _easeType = Ease.OutBounce;

    #region 변수
    private Material _irisMaterial;
    #endregion

    #region Fade
    private static readonly int _fadeID = Shader.PropertyToID("_Fade");
    private Tween _fadeTween;
    #endregion

    private void Awake()
    {
        _irisMaterial = _irisImage.material;
    }

    public void IrisIn(float duration = 1f, Action onComplete = null)
    {
        //이전 트윈 종료
        _fadeTween?.Kill();

        //페이드 인 트윈 시작
        _fadeTween = _irisMaterial
            .DOFloat(1f, _fadeID, duration)
            .SetEase(_easeType)
            .From(0f)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void IrisOut(float duration = 1f, Action onComplete = null)
    {
        //이전 트윈 종료
        _fadeTween?.Kill();

        //페이드 아웃 트윈 시작
        _fadeTween = _irisMaterial
            .DOFloat(0f, _fadeID, duration)
            .SetEase(_easeType)
            .From(1f)
            .OnComplete(() => onComplete?.Invoke());
    }
}