using System;
using UnityEngine;

/// <summary>
/// 씬 전환 UI를 관리하는 클래스
/// 씬 전환 간에 파괴되지 않는 DDOL 오브젝트
/// Iris Transition을 활용
/// </summary>
public class SceneTransitionUI : ShowHideUI
{
    [Header("UI Elements")]
    [SerializeField] private IrisFade _irisFade;

    #region Show, Hide
    public override void Show(float duration = 0.5F, Action onComplete = null)
    {
        //UI 활성화
        gameObject.SetActive(true);

        //아이리스 아웃 효과 실행
        _irisFade.IrisOut(duration, onComplete);
    }

    public override void Hide(float duration = 0.5F, Action onComplete = null)
    {
        //아이리스 인 효과 실행
        _irisFade.IrisIn(duration, () =>
        {
            //콜백 실행
            onComplete?.Invoke();

            //UI 비활성화
            gameObject.SetActive(false);
        });
    }
    #endregion
}