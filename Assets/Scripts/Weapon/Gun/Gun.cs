using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 총기 무기 클래스
/// GunData와 Stats를 사용하여 총기 특성 설정
/// 오브젝트 풀링을 통해 총알 궤적 이펙트와 총알 관리
/// 공격은 히트스캔 혹은 총알 발사
/// </summary>
public class Gun : Weapon
{
    #region 이펙트이면서 발사 지점
    [SerializeField] private ParticleSystem _muzzleFlash;
    #endregion

    #region 데이터
    private GunData _gunData;
    #endregion

    #region 변수
    private Camera _mainCamera;
    #endregion

    #region 스탯
    private Stats<GunStatType> _gunStats;
    public Stats<GunStatType> GunStats => _gunStats;
    #endregion

    #region 오브젝트 풀
    private ObjectPool<TrailRenderer> _trailPool;
    private ObjectPool<Bullet> _bulletPool;
    #endregion

    #region 타이머
    private float _nextTimeToFire = 0f;
    #endregion

    public override void Init(WeaponData weaponData, Player player)
    {
        base.Init(weaponData, player);

        _gunData = weaponData as GunData;
        _gunStats = new(_gunData.InitialStats);
        _mainCamera = Camera.main;
        InitPool();
    }

    private void InitPool()
    {
        _trailPool = new ObjectPool<TrailRenderer>(
            () => Instantiate(_gunData.TrailRendererPrefab),
            (trail) => trail.gameObject.SetActive(true),
            (trail) => trail.gameObject.SetActive(false),
            (trail) => Destroy(trail.gameObject),
            false
        );

        _bulletPool = new ObjectPool<Bullet>(
            () => Instantiate(_gunData.BulletPrefab),
            (bullet) => bullet.gameObject.SetActive(true),
            (bullet) => bullet.gameObject.SetActive(false),
            (bullet) => Destroy(bullet.gameObject),
            false
        );
    }

    private void Update()
    {
        HandleFire();
    }

    private void HandleFire()
    {
        if (!IsAttacking) return;
        if (_nextTimeToFire > Time.time) return;

        Fire();

        float fireSpeed = CombatUtility.CalculateFireSpeed(Player, this);
        float fireInterval = 1f / fireSpeed;

        _nextTimeToFire = Time.time + fireInterval;
    }

    #region 히트스캔 및 총알 발사
    private void Fire()
    {
        _muzzleFlash.Play();

        //카메라에서 총구까지의 거리보다 가까운 물체는 무시하기 위한 거리 계산
        float minDistance = Vector3.Distance(_mainCamera.transform.position, _muzzleFlash.transform.position);

        //화면 중앙을 발사 방향으로 설정
        Vector3 screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray fireRay = _mainCamera.ScreenPointToRay(screenCenter);

        //카메라와 총구 거리만큼 떨어진 지점에서 발사
        fireRay = new Ray(fireRay.origin + fireRay.direction * minDistance, fireRay.direction);
        float range = _gunStats.GetStat(GunStatType.Range).FinalValue;

        if (!Physics.Raycast(fireRay, out var fireHitInfo, range, _gunData.HitLayerMask))
        {
            fireHitInfo = new RaycastHit { point = fireRay.origin + fireRay.direction * range };
        }

        //히트스캔 발사 또는 총알 발사
        if (_gunData.IsHitScan)
        {
            FireHitscan(fireHitInfo);
        }
        else
        {
            Vector3 fireDirection = (fireHitInfo.point - _muzzleFlash.transform.position).normalized;
            FireBullet(fireDirection);
        }
    }

    //히트스캔 발사
    private void FireHitscan(RaycastHit fireHitInfo)
    {
        //데미지 처리
        if (fireHitInfo.collider != null && fireHitInfo.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            float distance = Vector3.Distance(_muzzleFlash.transform.position, fireHitInfo.point);
            ApplyDamage(damageable, distance);
        }

        //총알 궤적 이펙트 생성
        StartCoroutine(TrailCoroutine(_muzzleFlash.transform.position, fireHitInfo.point));
    }

    //총알 궤적 이펙트 코루틴
    private IEnumerator TrailCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        var trail = _trailPool.Get();
        trail.transform.position = startPosition;
        trail.Clear();
        trail.emitting = true;

        yield return null; // 한 프레임 대기하여 Trail Renderer가 시작 위치에 위치하도록 함

        float distance = Vector3.Distance(startPosition, endPosition);
        float speed = _gunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
        float duration = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            trail.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = endPosition;
        trail.emitting = false;
        StartCoroutine(DelayDisable(trail));
    }

    //총알 궤적 이펙트 지연 해제
    private IEnumerator DelayDisable(TrailRenderer trail)
    {
        yield return new WaitForSeconds(trail.time);
        _trailPool.Release(trail);
    }

    //총알 발사
    private void FireBullet(Vector3 fireDirection)
    {
        var bullet = _bulletPool.Get();
        bullet.OnBulletCollision += OnBulletCollision;
        bullet.transform.position = _muzzleFlash.transform.position;
        float speed = _gunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
        Vector3 initialSpeed = fireDirection * speed;
        float lifetime = _gunStats.GetStat(GunStatType.Range).FinalValue / speed;
        bullet.Fire(initialSpeed, lifetime);

        var trail = _trailPool.Get();
        if (trail != null)
        {
            trail.transform.SetParent(bullet.transform, false);
            trail.transform.localPosition = Vector3.zero;
            trail.Clear();
            trail.emitting = true;
        }
    }

    //총알 충돌 처리
    private void OnBulletCollision(Bullet bullet, Collision collision)
    {
        bullet.OnBulletCollision -= OnBulletCollision;

        var trail = bullet.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.transform.SetParent(null, true);
            trail.emitting = false;
            StartCoroutine(DelayDisable(trail));
        }

        _bulletPool.Release(bullet);

        if (collision != null)
        {
            var contact = collision.GetContact(0);

            if (contact.otherCollider.TryGetComponent<IDamageable>(out var damageable))
            {
                float distance = Vector3.Distance(bullet.SpawnPos, contact.point);
                ApplyDamage(damageable, distance);
            }
        }
    }

    //데미지 적용
    private void ApplyDamage(IDamageable damageable, float distance)
    {
        float damage = CombatUtility.CalculateBulletDamage(Player, this, distance);
        bool isCritical = CombatUtility.IsCritical(Player, this);
        if (isCritical)
        {
            damage = CombatUtility.CalculateCriticalDamage(Player, this, damage);
        }
        damageable.TakeDamage(damage);
    }
    #endregion
}