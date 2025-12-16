
/// <summary>
/// 공격 적중 시 퍼센트 데미지 효과 클래스
/// </summary>
public class HealthBasedDamageOnHitEffect : Effect
{
    #region 효과 변수
    private HealthBasedDamageOnHitEffectData _data;
    #endregion

    public HealthBasedDamageOnHitEffect(HealthBasedDamageOnHitEffectData effectData) : base(effectData)
    {
        //데이터 저장
        _data = effectData;
    }

    public override void Apply(Player player)
    {
        //플레이어의 히트 이벤트 등록
        player.OnHit += HandlePlayerHit;
    }

    public override void Remove(Player player)
    {
        //플레이어의 히트 이벤트 해제
        player.OnHit -= HandlePlayerHit;
    }

    private void HandlePlayerHit(PlayerDamageContext context)
    {
        //무한 루프 방지용: 이펙트로 인한 데미지일 경우 패스
        if (context.DamageSourceType == PlayerDamageSourceType.Effect) return;

        //목표 체력 가져오기
        //IsCurrentHealthBased에 따라 현재 체력 또는 최대 체력 선택
        float targetHealth = _data.IsCurrentHealthBased ? context.Enemy.Health.CurrentHealth : context.Enemy.Health.MaxHealth;

        //데미지 계산
        float damage = targetHealth * _data.DamageRate;

        //데미지 컨텍스트 생성
        PlayerDamageContext damageContext = new(
            context.Player,
            null,
            context.Enemy,
            damage,
            false,
            PlayerDamageSourceType.Effect
        );

        //적에게 데미지 적용
        //이때 isTriggerHit은 false로 설정하여 무한 루프 방지
        context.Enemy.TakeDamage(damageContext, false);
    }
}