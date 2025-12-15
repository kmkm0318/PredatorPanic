using UnityEngine;

/// <summary>
/// 총기 무기 클래스
/// GunData와 Stats를 사용하여 총기 특성 설정
/// 오브젝트 풀링을 통해 총알 궤적 이펙트와 총알 관리
/// 공격은 히트스캔 혹은 총알 발사
/// </summary>
public class Gun : Weapon
{
    #region 데이터
    private GunData _gunData;
    #endregion

    #region 레퍼런스 데이터
    private BulletManager _bulletManager;
    private TrailManager _trailManager;
    #endregion

    #region 스탯
    private Stats<GunStatType> _gunStats;
    public Stats<GunStatType> GunStats => _gunStats;
    #endregion

    #region 타이머
    private float _nextTimeToFire = 0f;
    #endregion

    public Gun(GunData gunData) : base(gunData)
    {
        _gunData = gunData;
    }

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);

        if (_gunData == null)
        {
            Debug.LogError("Gun Init Error: Invalid GunData");
            return;
        }

        _bulletManager = player.GameManager.BulletManager;
        _trailManager = player.GameManager.TrailManager;

        _gunStats = new(_gunData.InitialStats);
    }

    public override void HandleAttack()
    {
        HandleFire();
    }

    /// <summary>
    /// 총기 사격 처리
    /// PlayerAttack의 update에서 매 프레임 호출
    /// </summary>
    private void HandleFire()
    {
        //다음 발사 시간 전이면 반환
        if (_nextTimeToFire > Time.time) return;

        //가장 가까운 적 찾기
        //실패 시 패스
        if (!TryGetNearestEnemy(out var targetEnemy)) return;

        //적을 향한 방향 계산
        var directionToEnemy = (targetEnemy.CenterPosition - Player.CenterPosition).normalized;

        //총알들 발사
        FireBullets(directionToEnemy);

        //다음 발사 시간 계산
        float fireSpeed = CombatUtility.CalculateFireSpeed(Player, this);
        float fireInterval = 1f / fireSpeed;

        _nextTimeToFire = Time.time + fireInterval;
    }

    //가장 가까운 적 찾기
    private bool TryGetNearestEnemy(out Enemy targetEnemy)
    {
        //총기 사거리 가져오기
        var range = _gunStats.GetStat(GunStatType.Range).FinalValue;

        //적 레이어 마스크 가져오기
        var enemyLayerMask = DataManager.Instance.EnemyLayerMask;

        //가장 가까운 적 찾기
        var target = PhysicsUtility.GetNearestCollider(Player.transform.position, range, enemyLayerMask);

        //적 컴포넌트 가져오기 시도
        if (target != null && target.TryGetComponent(out targetEnemy))
        {
            //성공 시 true 반환
            return true;
        }
        else
        {
            //실패 시 false 반환
            targetEnemy = null;
            return false;
        }
    }

    #region 총알 발사
    //정면으로 총 발사
    //총알이 여러 발 발사되는 경우 처리
    private void FireBullets(Vector3 fireDirection)
    {
        int bulltCount = CombatUtility.CalculateBulletCount(Player, this);

        for (int i = 0; i < bulltCount; i++)
        {
            var curDirection = CombatUtility.GetSpreadDirection(fireDirection, i, bulltCount);
            FireBullet(curDirection);
        }
    }

    //총알 발사
    private void FireBullet(Vector3 fireDirection)
    {
        //총알 가져오기
        var bullet = _bulletManager.GetBullet(_gunData.BulletData);

        //위치 설정
        bullet.transform.position = Player.CenterPosition;

        //총알 발사를 위한 변수 설정
        var baseDamage = CombatUtility.CalculateBulletBaseDamage(Player, this);
        var speed = _gunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
        var range = _gunStats.GetStat(GunStatType.Range).FinalValue;
        var criticalRate = CombatUtility.CalculateCriticalRate(Player, this);
        var criticalDamageRate = CombatUtility.CalculateCriticalDamageRate(Player, this);
        var penetrationCount = CombatUtility.CalculatePenetrationCount(Player, this);
        var ricochetCount = CombatUtility.CalculateRicochetCount(Player, this);
        var explosionData = _gunData.ExplosionData;
        var enemyLayerMask = DataManager.Instance.EnemyLayerMask;

        //컨텍스트 생성
        BulletFireContext context = new(Player, this, fireDirection, baseDamage, speed, range, criticalRate, criticalDamageRate, penetrationCount, ricochetCount, explosionData, enemyLayerMask); ;

        //총알 발사
        bullet.Fire(context);

        //궤적 이펙트 부착
        if (_gunData.TrailData)
        {
            var trail = _trailManager.GetTrail(_gunData.TrailData);
            if (trail)
            {
                bullet.AttachTrail(trail);
            }
        }
    }
    #endregion
}