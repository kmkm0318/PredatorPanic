public class PlayerPresenter
{
    private Player _player;
    private PlayerUI _playerUI;

    // 생성자. GameUIManager에서 Player와 PlayerUI를 받아옴
    public PlayerPresenter(Player player, PlayerUI playerUI)
    {
        _player = player;
        _playerUI = playerUI;
        Init();
    }

    // 초기화 함수. 이벤트 등록 및 초기 UI 설정
    private void Init()
    {
        RegisterEvents();

        OnLevelChanged(_player.Level);
        OnExpChanged(_player.CurExp, _player.MaxExp);
        OnHealthChanged(_player.Health.CurrentHealth, _player.Health.MaxHealth);
    }

    // 해제 함수. 이벤트 해제
    public void Reset()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독/해제
    private void RegisterEvents()
    {
        if (_player != null)
        {
            _player.OnLevelChanged += OnLevelChanged;
            _player.OnExpChanged += OnExpChanged;
            _player.Health.OnHealthChanged += OnHealthChanged;
        }
    }

    private void UnregisterEvents()
    {
        if (_player != null)
        {
            _player.OnLevelChanged -= OnLevelChanged;
            _player.OnExpChanged -= OnExpChanged;
            _player.Health.OnHealthChanged -= OnHealthChanged;
        }
    }
    #endregion

    #region UI 업데이트 함수
    private void OnHealthChanged(float cur, float max)
    {
        _playerUI.SetHealth(cur, max);
    }

    private void OnExpChanged(float cur, float max)
    {
        _playerUI.SetExp(cur, max);
    }

    private void OnLevelChanged(int level)
    {
        _playerUI.SetLevel(level);
    }
    #endregion
}