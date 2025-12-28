/// <summary>
/// 데미지 텍스트 프리젠터
/// MVP 패턴에서 데미지 텍스트 UI와 게임 로직 간의 상호작용을 관리
/// </summary>
public class DamageTextPresenter : IPresenter
{
    #region 참조 변수
    private Player _player;
    private DamageTextUI _damageTextUI;
    #endregion

    public DamageTextPresenter(Player player, DamageTextUI damageTextUI)
    {
        _player = player;
        _damageTextUI = damageTextUI;
    }

    public void Init()
    {
        RegisterEvents();
    }

    public void Reset()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        if (_player)
        {
            _player.OnHit += OnPlayerHit;
        }
    }

    private void UnregisterEvents()
    {
        if (_player)
        {
            _player.OnHit -= OnPlayerHit;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void OnPlayerHit(PlayerDamageContext context)
    {
        //플레이어, 적 위치를 통해 위치 및 방향 계산
        var player = context.Player;
        var enemy = context.Enemy;
        var pos = enemy.transform.position;
        var forward = (pos - player.transform.position).normalized;

        //데미지 텍스트 UI에 데미지 텍스트 표시 요청
        _damageTextUI.ShowDamageText(
            pos,
            forward,
            context.Damage,
            context.IsCritical,
            DamageTextType.Normal //추후 타입 확장 가능
        );
    }
    #endregion
}