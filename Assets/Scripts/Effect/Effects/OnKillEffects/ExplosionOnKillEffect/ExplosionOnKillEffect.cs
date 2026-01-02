/// <summary>
/// 적 처치 시 폭발 효과 클래스
/// </summary>
public class ExplosionOnKillEffect : Effect
{
    #region 데이터
    private ExplosionOnKillEffectData _data;
    #endregion

    #region 레퍼런스
    private ExplosionManager _explosionManager;
    #endregion

    public ExplosionOnKillEffect(EffectData effectData) : base(effectData)
    {
        _data = effectData as ExplosionOnKillEffectData;
    }

    public override void Apply(Player player)
    {
        //폭발 매니저 레퍼런스 저장
        _explosionManager = player.GameManager.ExplosionManager;

        //플레이어의 킬 이벤트 등록
        player.OnKill += HandlePlayerKill;
    }

    public override void Remove(Player player)
    {
        //플레이어의 킬 이벤트 해제
        player.OnKill -= HandlePlayerKill;
    }

    private void HandlePlayerKill(PlayerDamageContext context)
    {
        //확률 검사 실패 시 패스
        if (!_data.Chance.ChanceTest()) return;

        //폭발 생성
        var explosion = _explosionManager.GetExplosion(_data.ExplosionData);

        //폭발이 없으면 패스
        if (explosion == null) return;

        //플레이어 가져오기
        var player = context.Player;

        //플레이어 공격력 가져오기
        var playerAttack = player.PlayerStats.GetStat(PlayerStatType.Attack).FinalValue;

        //폭발 데미지 계산
        var damage = CombatUtility.CalculateAttackDamage(playerAttack, _data.Damage);

        //적 레이어 마스크 가져오기
        var enemyLayerMask = DataManager.Instance.EnemyLayerMask;

        //폭발 위치는 적의 중앙 위치
        var origin = context.Enemy.CenterPosition;

        //폭발 폭파 컨텍스트 생성
        ExplosionExplodeContext explosionContext = new(
            player,
            null,
            origin,
            damage,
            _data.Radius,
            0f, //치명타 확률은 0
            1f, //치명타 데미지 배율은 1
            enemyLayerMask
        );

        //폭발 실행
        explosion.Explode(explosionContext);
    }
}