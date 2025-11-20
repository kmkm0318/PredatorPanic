using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 플레이어 UI 클래스
/// 플레이어의 체력, 경험치, 레벨 표시 담당
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class PlayerUI : MonoBehaviour
{
    //UIDocument는 PlayerUI.uxml을 사용해야 함
    private UIDocument _uiDocument;
    private Label _levelLabel;
    private ProgressBar _healthBar;
    private ProgressBar _expBar;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        _levelLabel = root.Q<Label>("LevelLabel");
        _healthBar = root.Q<ProgressBar>("HealthBar");
        _expBar = root.Q<ProgressBar>("ExpBar");
    }

    #region UI 업데이트 함수
    public void SetHealth(float cur, float max)
    {
        _healthBar.value = cur;
        _healthBar.highValue = max;
        _healthBar.title = $"{Mathf.CeilToInt(cur)} / {Mathf.CeilToInt(max)}";
    }

    public void SetExp(float cur, float max)
    {
        _expBar.value = cur;
        _expBar.highValue = max;
        _expBar.title = $"{Mathf.CeilToInt(cur)} / {Mathf.CeilToInt(max)}";
    }

    public void SetLevel(int level)
    {
        _levelLabel.text = $"Lv.{level}";
    }
    #endregion
}