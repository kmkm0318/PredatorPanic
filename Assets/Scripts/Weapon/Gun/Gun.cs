using System.Collections;
using UnityEngine;

/// <summary>
/// 총기 무기 클래스
/// GunData와 Stats를 사용하여 총기 특성 설정
/// 오브젝트 풀링을 통해 총알 궤적 이펙트와 총알 관리
/// 공격은 히트스캔 혹은 총알 발사
/// </summary>
public class Gun : Weapon
{
    #region 비주얼 컴포넌트
    [SerializeField] private GunVisual _gunVisual;
    [SerializeField] private ParticleSystem _muzzleFlash;
    #endregion

    #region 데이터
    private GunData _gunData;
    #endregion

    #region 레퍼런스 데이터
    private BulletManager _bulletManager;
    private TrailManager _trailManager;
    #endregion

    #region 변수
    private Camera _mainCamera;
    #endregion

    #region 스탯
    private Stats<GunStatType> _gunStats;
    public Stats<GunStatType> GunStats => _gunStats;
    #endregion

    #region 타이머
    private float _nextTimeToFire = 0f;
    #endregion

    public override void Init(WeaponData weaponData, Player player)
    {
        base.Init(weaponData, player);

        _gunData = weaponData as GunData;

        if (_gunData == null)
        {
            Debug.LogError("Gun Init Error: Invalid GunData");
            return;
        }

        _bulletManager = player.GameManager.BulletManager;
        _trailManager = player.GameManager.TrailManager;

        _gunStats = new(_gunData.InitialStats);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleFire();
    }

    private void HandleFire()
    {
        //공격 중이 아니면 반환
        if (!IsAttacking) return;

        //다음 발사 시간 전이면 반환
        if (_nextTimeToFire > Time.time) return;

        //총 발사
        Fire();

        //다음 발사 시간 계산
        float fireSpeed = CombatUtility.CalculateFireSpeed(Player, this);
        float fireInterval = 1f / fireSpeed;

        _nextTimeToFire = Time.time + fireInterval;

        //발사 애니메이션 재생
        _gunVisual.PlayFire(fireSpeed);
    }

    #region 히트스캔 및 총알 발사
    private void Fire()
    {
        _muzzleFlash.Play();

        //카메라에서 무기까지의 거리보다 가까운 물체는 무시하기 위한 거리 계산
        float minDistance = Vector3.Distance(_mainCamera.transform.position, transform.position);

        //화면 중앙을 발사 방향으로 설정
        Vector3 screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray fireRay = _mainCamera.ScreenPointToRay(screenCenter);

        //minDistance만큼 떨어진 지점에서 발사
        fireRay = new Ray(fireRay.origin + fireRay.direction * minDistance, fireRay.direction);
        float range = _gunStats.GetStat(GunStatType.Range).FinalValue;

        if (!Physics.Raycast(fireRay, out var fireHitInfo, range, _gunData.HitLayerMask))
        {
            fireHitInfo = new RaycastHit { point = fireRay.origin + fireRay.direction * range };
        }

        #region 히트스캔 사용하지 않음
        // //히트스캔 발사 또는 총알 발사
        // if (_gunData.IsHitScan)
        // {
        //     FireHitscan(fireHitInfo);
        // }
        // else
        // {
        //     Vector3 fireDirection = (fireHitInfo.point - _muzzleFlash.transform.position).normalized;
        //     FireBullet(fireDirection);
        // }
        #endregion

        Vector3 fireDirection = (fireHitInfo.point - _muzzleFlash.transform.position).normalized;

        int bulltCount = CombatUtility.CalculateBulletCount(Player, this);

        for (int i = 0; i < bulltCount; i++)
        {
            var curDirection = CombatUtility.GetSpreadDirection(fireDirection, i, bulltCount);
            FireBullet(curDirection);
        }
    }

    #region 히트스캔 사용하지 않음
    // //히트스캔 발사
    // private void FireHitscan(RaycastHit fireHitInfo)
    // {
    //     //데미지 처리
    //     if (fireHitInfo.collider != null && fireHitInfo.collider.TryGetComponent<IDamageable>(out var damageable))
    //     {
    //         float distance = Vector3.Distance(_muzzleFlash.transform.position, fireHitInfo.point);
    //         ApplyDamage(damageable, distance);
    //     }

    //     //총알 궤적 이펙트 생성
    //     StartCoroutine(TrailCoroutine(_muzzleFlash.transform.position, fireHitInfo.point));
    // }

    // //총알 궤적 이펙트 코루틴
    // private IEnumerator TrailCoroutine(Vector3 startPosition, Vector3 endPosition)
    // {
    //     var trail = _trailManager.GetTrail(_gunData.TrailData);
    //     var trailRenderer = trail.TrailRenderer;
    //     trailRenderer.transform.position = startPosition;
    //     trailRenderer.Clear();
    //     trailRenderer.emitting = true;

    //     yield return null; // 한 프레임 대기하여 Trail Renderer가 시작 위치에 위치하도록 함

    //     float distance = Vector3.Distance(startPosition, endPosition);
    //     float speed = _gunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
    //     float duration = distance / speed;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < duration)
    //     {
    //         trailRenderer.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     trailRenderer.transform.position = endPosition;
    //     trailRenderer.emitting = false;
    //     StartCoroutine(DelayDisable(trail));
    // }
    #endregion

    //총알 발사
    private void FireBullet(Vector3 fireDirection)
    {
        //총알 가져오기
        var bullet = _bulletManager.GetBullet(_gunData.BulletData);

        //위치 설정
        bullet.transform.position = _muzzleFlash.transform.position;

        //총알 발사를 위한 변수 설정
        float baseDamage = CombatUtility.CalculateBulletBaseDamage(Player, this);
        float speed = _gunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
        float range = _gunStats.GetStat(GunStatType.Range).FinalValue;
        float criticalRate = CombatUtility.CalculateCriticalRate(Player, this);
        float criticalDamageRate = CombatUtility.CalculateCriticalDamageRate(Player, this);
        int penetrationCount = CombatUtility.CalculatePenetrationCount(Player, this);
        int ricochetCount = CombatUtility.CalculateRicochetCount(Player, this);

        //컨텍스트 생성
        BulletFireContext context = new()
        {
            Gun = this,
            FireDirection = fireDirection,
            BaseDamage = baseDamage,
            Speed = speed,
            Range = range,
            CriticalRate = criticalRate,
            CriticalDamageRate = criticalDamageRate,
            PenetrationCount = penetrationCount,
            RicochetCount = ricochetCount,
            HitLayerMask = _gunData.HitLayerMask
        };

        //총알 발사
        bullet.Fire(context);

        //궤적 이펙트 부착
        var trail = _trailManager.GetTrail(_gunData.TrailData);

        if (trail != null)
        {
            bullet.AttachTrail(trail);
        }
    }
    #endregion
}