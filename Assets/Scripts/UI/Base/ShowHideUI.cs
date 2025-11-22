using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Show, Hide를 가지는 UI의 추상 클래스
/// UI의 표시 및 숨김 애니메이션을 관리합니다.
/// 애니메이션은 TimeScale의 영향을 받지 않도록 설정되어 있습니다.
/// </summary>
public abstract class ShowHideUI : MonoBehaviour, IShowHide
{
    [SerializeField] protected CanvasGroup _background;
    [SerializeField] protected RectTransform _panel;
    [SerializeField] protected Vector3 _showPos = Vector3.zero;
    [SerializeField] protected Vector3 _hidePos = new(0, -1000, 0);
    [SerializeField] protected Ease _ease = Ease.OutBack;

    private Sequence _currentSequence;

    /// <summary>
    /// UI를 보여줍니다.
    /// </summary>
    /// <param name="duration">애니메이션 지속 시간</param>
    /// <param name="onComplete">애니메이션 완료 후 호출될 함수</param>
    public virtual void Show(float duration = 0.5f, Action onComplete = null)
    {
        gameObject.SetActive(true);
        _currentSequence?.Kill(true);

        bool hasBackground = _background != null;
        bool hasPanel = _panel != null;

        if (!hasBackground && !hasPanel)
        {
            onComplete?.Invoke();
            return;
        }

        _currentSequence = DOTween.Sequence();
        _currentSequence.SetUpdate(true);

        if (hasBackground)
        {
            _background.alpha = 0f;
            var backgroundTween = _background.DOFade(1f, duration);
            _currentSequence.Append(backgroundTween);
        }

        if (hasPanel)
        {
            var panelTween = _panel.DOAnchorPos(_showPos, duration).From(_hidePos).SetEase(_ease);

            if (hasBackground)
            {
                _currentSequence.Join(panelTween);
            }
            else
            {
                _currentSequence.Append(panelTween);
            }
        }

        _currentSequence.OnComplete(() => onComplete?.Invoke());
        _currentSequence.Play();
    }

    /// <summary>
    /// UI를 숨깁니다.
    /// </summary>
    /// <param name="duration">애니메이션 지속 시간</param>
    /// <param name="onComplete">애니메이션 완료 후 호출될 함수</param>
    public virtual void Hide(float duration = 0.5F, Action onComplete = null)
    {
        _currentSequence?.Kill(true);

        bool hasBackground = _background != null;
        bool hasPanel = _panel != null;

        if (!hasBackground && !hasPanel)
        {
            onComplete?.Invoke();
            return;
        }

        _currentSequence = DOTween.Sequence();
        _currentSequence.SetUpdate(true);

        if (hasBackground)
        {
            _background.alpha = 1f;
            var backgroundTween = _background.DOFade(0f, duration);
            _currentSequence.Append(backgroundTween);
        }

        if (hasPanel)
        {
            var panelTween = _panel.DOAnchorPos(_hidePos, duration).From(_showPos).SetEase(_ease);

            if (hasBackground)
            {
                _currentSequence.Join(panelTween);
            }
            else
            {
                _currentSequence.Append(panelTween);
            }
        }

        _currentSequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });
        _currentSequence.Play();
    }
}