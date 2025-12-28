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
        //이전 시퀀스 종료
        _currentSequence?.Kill(true);

        //UI 활성화
        gameObject.SetActive(true);

        //배경 및 패널 존재 여부 확인
        bool hasBackground = _background != null;
        bool hasPanel = _panel != null;

        //존재하지 않으면 바로 완료 호출
        if (!hasBackground && !hasPanel)
        {
            onComplete?.Invoke();
            return;
        }

        if (duration <= 0f)
        {
            //배경 초기화
            if (hasBackground)
            {
                _background.alpha = 1f;
            }

            //패널 위치 초기화
            if (hasPanel)
            {
                _panel.anchoredPosition = _showPos;
            }

            //이벤트 호출
            onComplete?.Invoke();

            return;
        }

        //애니메이션 시퀀스 생성
        _currentSequence = DOTween.Sequence();
        _currentSequence.SetUpdate(true);

        //배경 애니메이션 추가
        if (hasBackground)
        {
            _background.alpha = 0f;
            var backgroundTween = _background.DOFade(1f, duration);
            _currentSequence.Append(backgroundTween);
        }

        //패널 애니메이션 추가
        if (hasPanel)
        {
            var panelTween = _panel.DOAnchorPos(_showPos, duration).From(_hidePos).SetEase(_ease);

            if (hasBackground)
            {
                //배경이 있으면
                _currentSequence.Join(panelTween);
            }
            else
            {
                //배경이 없으면
                _currentSequence.Append(panelTween);
            }
        }

        // 완료 콜백 설정
        _currentSequence.OnComplete(() => onComplete?.Invoke());

        // 재생
        _currentSequence.Play();
    }

    /// <summary>
    /// UI를 숨깁니다.
    /// </summary>
    /// <param name="duration">애니메이션 지속 시간</param>
    /// <param name="onComplete">애니메이션 완료 후 호출될 함수</param>
    public virtual void Hide(float duration = 0.5F, Action onComplete = null)
    {
        //이전 시퀀스 종료
        _currentSequence?.Kill(true);

        //배경 및 패널 존재 여부 확인
        bool hasBackground = _background != null;
        bool hasPanel = _panel != null;

        //존재하지 않으면 바로 완료 호출
        if (!hasBackground && !hasPanel)
        {
            onComplete?.Invoke();
            return;
        }

        //지속 시간이 0 이하일 경우 즉시 숨기기
        if (duration <= 0f)
        {
            //배경 초기화
            if (hasBackground)
            {
                _background.alpha = 0f;
            }

            //패널 위치 초기화
            if (hasPanel)
            {
                _panel.anchoredPosition = _hidePos;
            }

            //즉시 숨기기
            gameObject.SetActive(false);

            //이벤트 호출
            onComplete?.Invoke();

            return;
        }

        //애니메이션 시퀀스 생성
        _currentSequence = DOTween.Sequence();
        _currentSequence.SetUpdate(true);

        //배경 애니메이션 추가
        if (hasBackground)
        {
            _background.alpha = 1f;
            var backgroundTween = _background.DOFade(0f, duration);
            _currentSequence.Append(backgroundTween);
        }

        //패널 애니메이션 추가
        if (hasPanel)
        {
            var panelTween = _panel.DOAnchorPos(_hidePos, duration).From(_showPos).SetEase(_ease);

            if (hasBackground)
            {
                //배경이 있으면
                _currentSequence.Join(panelTween);
            }
            else
            {
                //배경이 없으면
                _currentSequence.Append(panelTween);
            }
        }

        // 완료 콜백 설정
        _currentSequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });

        // 재생
        _currentSequence.Play();
    }
}