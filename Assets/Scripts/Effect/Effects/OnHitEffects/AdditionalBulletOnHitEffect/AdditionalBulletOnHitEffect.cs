using UnityEngine;

/// <summary>
/// 공격 적중 시 추가 탄환 발사 효과 클래스
/// </summary>
public class AdditionalBulletOnHitEffect : Effect
{
    #region 데이터
    private readonly AdditionalBulletOnHitEffectData _data;
    #endregion

    #region 레퍼런스
    private BulletManager bulletManager;
    private TrailManager trailManager;
    #endregion

    public AdditionalBulletOnHitEffect(AdditionalBulletOnHitEffectData data) : base(data)
    {
        _data = data;
    }

    public override void Apply(Player player)
    {
        // BulletManager 참조 가져오기
        bulletManager = player.GameManager.BulletManager;

        // TrailManager 참조 가져오기
        trailManager = player.GameManager.TrailManager;

        // 플레이어의 히트 이벤트 등록
        player.OnHit += HandlePlayerHit;
    }

    public override void Remove(Player player)
    {
        // 플레이어의 히트 이벤트 해제
        player.OnHit -= HandlePlayerHit;
    }

    private void HandlePlayerHit(PlayerDamageContext context)
    {
        // 무한 루프 방지용: 이펙트로 인한 데미지일 경우 패스
        if (context.DamageSourceType == PlayerDamageSourceType.Effect) return;

        // 확률 검사 실패 시 반환
        if (!_data.Chance.ChanceTest()) return;

        // 총알 가져오기
        var bullet = bulletManager.GetBullet(_data.BulletData);

        // 총알이 없으면 반환
        if (bullet == null) return;

        // 궤적 가져오기
        var trail = trailManager.GetTrail(_data.TrailData);

        // 가장 가까운 적 찾기
        var origin = context.Player.CenterPosition;
        var enemyLayerMask = DataManager.Instance.EnemyLayerMask;
        var targetEnemyCollider = PhysicsUtility.GetNearestCollider(origin, _data.Range, enemyLayerMask);

        //적이 없으면 반환
        if (targetEnemyCollider == null || !targetEnemyCollider.TryGetComponent<Enemy>(out var targetEnemy)) return;

        // 발사 방향 계산
        Vector3 fireDirection;

        if (_data.BulletData.IsHoming)
        {
            // 호밍 총알은 위 방향으로 발사
            fireDirection = Vector3.up;
        }
        else
        {
            // 기본 총알은 적을 향한 방향 계산
            fireDirection = (targetEnemy.CenterPosition - origin).normalized;
        }

        // 총알 발사 컨텍스트 생성
        BulletFireContext fireContext = new(
            context.Player,
            targetEnemy, //첫 목표 적
            null, //무기는 null
            trail,
            origin,
            fireDirection,
            _data.Damage,
            _data.Speed,
            _data.Range,
            0, //크리티컬 확률 없음
            1f, //크리티컬 1
            0, //관통 없음
            0, //튕김 없음
            null, //폭발 없음
            enemyLayerMask
        );

        // 총알 발사
        bullet.Fire(fireContext);
    }
}