using TMPro;
using UnityEngine;

/// <summary>
/// 보스 체력 UI 클래스
/// </summary>
public class BossHealthUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _bossNameText;
    [SerializeField] private Progressbar _healthBar;


    /// <summary>
    /// 보스 체력 UI 초기화
    /// </summary>
    public void Init(string bossName, float cur, float max)
    {
        SetBossName(bossName);
        SetHealth(cur, max);
    }

    /// <summary>
    /// 보스 이름 설정
    /// </summary>
    public void SetBossName(string name)
    {
        if (_bossNameText)
        {
            _bossNameText.text = name;
        }
    }

    /// <summary>
    /// 보스 체력 설정
    /// </summary>
    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (_healthBar)
        {
            _healthBar.SetValue(currentHealth, maxHealth);
        }
    }
}