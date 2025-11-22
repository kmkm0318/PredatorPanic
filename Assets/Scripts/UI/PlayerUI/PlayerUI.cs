using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어에게 필요한 UI를 관리하는 클래스
/// </summary>
public class PlayerUI : MonoBehaviour
{
    #region UI 요소
    [SerializeField] private TMP_Text _levelLabel;
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
        _levelLabel.text = $"Lv.{level}";
    }
    #endregion
}