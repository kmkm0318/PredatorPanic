using System;
using System.Collections;
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

    #region 이벤트
    public static event Action<DamageText> OnAnyDamageTextDone;
    #endregion

    public void Show(DamageTextData data, float damage, bool isCritical)
    {
        //데이터 할당
        Data = data;

        // 데미지 텍스트 설정
        _damageText.text = damage.ToString("0.#");
        _damageText.color = isCritical ? Data.CriticalColor : Data.NormalColor;

        // 애니메이션 시작
        StartCoroutine(AnimationCoroutine());
    }

    #region 애니메이션 코루틴
    //데미지 텍스트 애니메이션 코루틴
    private IEnumerator AnimationCoroutine()
    {
        //이동 및 페이드 코루틴 실행
        yield return StartCoroutine(MoveCoroutine());
        yield return StartCoroutine(FadeCoroutine());

        //애니메이션 완료 이벤트 호출
        OnAnyDamageTextDone?.Invoke(this);
    }

    //움직임 코루틴
    private IEnumerator MoveCoroutine()
    {
        //시작, 목표 위치 및 색
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0, Data.MoveDistance, 0); // 위로 이동

        //시간
        float elapsedTime = 0f;
        float duration = Data.MoveDuration;

        while (elapsedTime < duration)
        {
            // 비율
            float t = elapsedTime / duration;

            // 위치 보간
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // 시간 증가
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치
        transform.localPosition = targetPosition;
    }

    //페이드 아웃 코루틴
    private IEnumerator FadeCoroutine()
    {
        //시작, 목표 색
        Color startColor = _damageText.color;
        Color targetColor = new(startColor.r, startColor.g, startColor.b, 0); // 투명해짐

        //애니메이션 시간
        float elapsedTime = 0f;
        float duration = Data.FadeDuration;

        while (elapsedTime < duration)
        {
            // 비율
            float t = elapsedTime / duration;

            // 색상 보간
            _damageText.color = Color.Lerp(startColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 색상 설정
        _damageText.color = targetColor;
    }
    #endregion
}