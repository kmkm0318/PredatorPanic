using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어 스탯 아이템 클래스
/// </summary>
public class PlayerStatItem : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text _statNameText;
    [SerializeField] private TMP_Text _statValueText;

    #region 스탯 텍스트 설정
    /// <summary>
    /// 스탯 설정 함수
    /// </summary>
    public void SetStat(string statName, string statValue)
    {
        SetStatName(statName);
        SetStatValue(statValue);
    }

    /// <summary>
    /// 스탯 이름 설정 함수
    /// </summary>
    public void SetStatName(string statName)
    {
        _statNameText.text = statName;
    }

    /// <summary>
    /// 스탯 값 설정 함수
    /// </summary>
    public void SetStatValue(string statValue)
    {
        _statValueText.text = statValue;
    }
    #endregion
}