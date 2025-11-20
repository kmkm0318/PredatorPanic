using UnityEngine;

/// <summary>
/// 플레이어 UI 프리젠터 클래스
/// 플레이어 UI와 플레이어 데이터 연결 담당
/// </summary>
[RequireComponent(typeof(PlayerUI))]
public class PlayerUIPresenter : MonoBehaviour
{
    //PlayerUI와 같은 게임 오브젝트에 붙어야 함
    private PlayerUI _playerUI;
    private Player _player;

    private void Awake()
    {
        _playerUI = GetComponent<PlayerUI>();
    }

    //외부에서 플레이어를 지정해주어야 함
    public void Init(Player player)
    {
        _player = player;

        _playerUI.SetLevel(_player.Level);
        _playerUI.SetExp(_player.CurExp, _player.MaxExp);
        _playerUI.SetHealth(_player.Health.CurrentHealth, _player.Health.MaxHealth);

        _player.OnLevelChanged += HandleLevelChanged;
        _player.OnExpChanged += HandleExpChanged;
        _player.Health.OnHealthChanged += HandleHealthChanged;
    }

    //구독 해제
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnLevelChanged -= HandleLevelChanged;
            _player.OnExpChanged -= HandleExpChanged;
            _player.Health.OnHealthChanged -= HandleHealthChanged;
        }
    }

    #region UI 업데이트 함수
    private void HandleLevelChanged(int level)
    {
        _playerUI.SetLevel(level);
    }

    private void HandleExpChanged(float cur, float max)
    {
        _playerUI.SetExp(cur, max);
    }

    private void HandleHealthChanged(float cur, float max)
    {
        _playerUI.SetHealth(cur, max);
    }
    #endregion
}