public class PlayerPresenter : IPresenter
{
    private Player _player;
    private PlayerUI _playerUI;

    // GameUIManager에서 Player와 PlayerUI를 받아옴
    public PlayerPresenter(Player player, PlayerUI playerUI)
    {
        _player = player;
        _playerUI = playerUI;
    }

    // 초기화 함수. 이벤트 등록 및 초기 UI 설정
    public void Init()
    {
        RegisterEvents();

        OnLevelChanged(_player.Level);
        OnExpChanged(_player.CurExp, _player.MaxExp);
        OnHealthChanged(_player.Health.CurrentHealth, _player.Health.MaxHealth);
        OnToothChanged(_player.Tooth);
        OnDNAChanged(_player.DNA);
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
            _player.OnToothChanged += OnToothChanged;
            _player.OnDNAChanged += OnDNAChanged;
        }
    }

    private void UnregisterEvents()
    {
        if (_player != null)
        {
            _player.OnLevelChanged -= OnLevelChanged;
            _player.OnExpChanged -= OnExpChanged;
            _player.Health.OnHealthChanged -= OnHealthChanged;
            _player.OnToothChanged -= OnToothChanged;
            _player.OnDNAChanged -= OnDNAChanged;
        }
    }
    #endregion

    #region 이벤트 핸들러 함수
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

    private void OnToothChanged(int tooth)
    {
        _playerUI.SetTooth(tooth);
    }

    private void OnDNAChanged(int dna)
    {
        _playerUI.SetDNA(dna);
    }
    #endregion
}