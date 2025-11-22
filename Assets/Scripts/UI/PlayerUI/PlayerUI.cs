using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어에게 필요한 UI를 관리하는 클래스
/// </summary>
public class PlayerUI : MonoBehaviour
{
    #region UI 요소
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _toothText;
    [SerializeField] private TMP_Text _dnaText;
    [SerializeField] private Progressbar _healthBar;
    [SerializeField] private Progressbar _expBar;
    #endregion


    #region UI 업데이트 함수
    public void SetHealth(float cur, float max)
    {
        _healthBar.SetValue(cur, max);
    }

    public void SetExp(float cur, float max)
    {
        _expBar.SetValue(cur, max);
    }

    public void SetLevel(int level)
    {
        _levelText.text = $"Lv.{level}";
    }

    public void SetTooth(int toothCount)
    {
        _toothText.text = toothCount.ToString();
    }

    public void SetDNA(int dnaCount)
    {
        _dnaText.text = dnaCount.ToString();
    }
    #endregion
}