using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 데미지 텍스트를 위한 UI 컴포넌트
/// </summary>
public class DamageText : MonoBehaviour
{
    #region 참조 변수
    [SerializeField] private TMP_Text _damageText;
    #endregion

    #region 데이터
    public DamageTextData Data { get; private set; }
    #endregion

    #region 변수
    private Sequence _showSequence;
    #endregion

    #region 이벤트
    public static event Action<DamageText> OnAnyDamageTextDone;
    #endregion

    public void Show(DamageTextData data, float damage, bool isCritical)
    {
        //데이터 할당
        Data = data;

        // 데미지 텍스트 설정
        _damageText.text = damage.GetFormatedNumber();
        _damageText.color = isCritical ? Data.CriticalColor : Data.NormalColor;

        // 애니메이션 시작
        PlayShowAnimation();
    }

    private void PlayShowAnimation()
    {
        //이전 시퀀스가 있으면 종료
        _showSequence?.Kill();

        //투명도 초기화
        _damageText.alpha = 1f;

        //새 시퀀스 생성
        _showSequence = DOTween.Sequence();

        //위로 이동 애니메이션 추가
        _showSequence.Append(transform.DOLocalMoveY(transform.localPosition.y + Data.MoveDistance, Data.MoveDuration).SetEase(Data.MoveEase));

        //페이드 아웃 애니메이션 추가
        _showSequence.Append(_damageText.DOFade(0f, Data.FadeDuration));

        //애니메이션 완료 시 이벤트 호출
        _showSequence.OnComplete(() => OnAnyDamageTextDone?.Invoke(this));
    }
}