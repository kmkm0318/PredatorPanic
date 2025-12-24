using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 라운드 UI 클래스
/// </summary>
public class RoundUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _roundTimerText;

    #region 라운드 텍스트
    /// <summary>
    /// 라운드 설정
    /// </summary>
    public void SetRoundText(int round)
    {
        _roundText.text = $"Round {round}";
    }

    /// <summary>
    /// 라운드 텍스트 표시/숨기기
    /// </summary>
    public void ShowRoundText(bool isShow)
    {
        _roundText.gameObject.SetActive(isShow);
    }
    #endregion

    #region 라운드 시간 텍스트
    /// <summary>
    /// 라운드 시간 설정
    /// </summary>
    public void SetRoundTimerText(float time)
    {
        _roundTimerText.text = Mathf.FloorToInt(time).ToString("0");
    }

    /// <summary>
    /// 라운드 시간 표시/숨기기
    /// </summary>
    public void ShowRoundTimerText(bool isShow)
    {
        _roundTimerText.gameObject.SetActive(isShow);
    }
    #endregion
}