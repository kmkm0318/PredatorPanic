using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 플레이어에게 필요한 UI를 관리하는 클래스
/// GameScene에서 GameUI 오브젝트에 붙어 있어야 함
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class PlayerUI : MonoBehaviour
{
    #region UI 요소
    private UIDocument _uiDocument;
    private Label _levelLabel;
    private ProgressBar _healthBar;
    private ProgressBar _expBar;
    #endregion

    #region 플레이어
    private Player _player;
    #endregion

    // 초기화
    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();

        var root = _uiDocument.rootVisualElement;
        var playerUI = root.Q<VisualElement>("PlayerUI");

        _levelLabel = playerUI.Q<Label>("LevelLabel");
        _healthBar = playerUI.Q<ProgressBar>("HealthBar");
        _expBar = playerUI.Q<ProgressBar>("ExpBar");
    }

    // Player를 외부(UIManager)에서 주입
    public void Init(Player player)
    {
        UnregisterEvents();

        _player = player;

        SetLevel(_player.Level);
        SetExp(_player.CurExp, _player.MaxExp);
        SetHealth(_player.Health.CurrentHealth, _player.Health.MaxHealth);

        RegisterEvents();
    }

    // 이벤트 정리
    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독/해제
    private void RegisterEvents()
    {
        if (_player != null)
        {
            _player.OnLevelChanged += SetLevel;
            _player.OnExpChanged += SetExp;
            _player.Health.OnHealthChanged += SetHealth;
        }
    }

    private void UnregisterEvents()
    {
        if (_player != null)
        {
            _player.OnLevelChanged -= SetLevel;
            _player.OnExpChanged -= SetExp;
            _player.Health.OnHealthChanged -= SetHealth;
        }
    }
    #endregion

    #region UI 업데이트 함수
    private void SetHealth(float cur, float max)
    {
        _healthBar.value = cur;
        _healthBar.highValue = max;
        _healthBar.title = $"{Mathf.CeilToInt(cur)} / {Mathf.CeilToInt(max)}";
    }

    private void SetExp(float cur, float max)
    {
        _expBar.value = cur;
        _expBar.highValue = max;
        _expBar.title = $"{Mathf.CeilToInt(cur)} / {Mathf.CeilToInt(max)}";
    }

    private void SetLevel(int level)
    {
        _levelLabel.text = $"Lv.{level}";
    }
    #endregion
}