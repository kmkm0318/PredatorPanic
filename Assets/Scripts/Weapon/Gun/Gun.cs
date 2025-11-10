using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 총기 무기 클래스
/// GunData를 사용하여 총기 특성 설정
/// 오브젝트 풀링을 통해 총알 궤적 이펙트 최적화
/// 공격은 히트스캔을 사용
/// </summary>
public class Gun : Weapon
{
    [SerializeField] private ParticleSystem _shootSystem;
    private GunData _gunData;
    private Camera _mainCamera;
    private ObjectPool<TrailRenderer> _trailPool;
    private float _nextTimeToFire = 0f;

    public override void Init(WeaponData weaponData)
    {
        base.Init(weaponData);

        _gunData = weaponData as GunData;
        _mainCamera = Camera.main;
        InitPool();
    }

    private void InitPool()
    {
        _trailPool = new ObjectPool<TrailRenderer>(
            createFunc: () =>
            {
                var trailInstance = Instantiate(_gunData.TrailRendererPrefab);
                trailInstance.gameObject.SetActive(false);
                return trailInstance;
            },
            actionOnGet: (trail) => trail.gameObject.SetActive(true),
            actionOnRelease: (trail) => trail.gameObject.SetActive(false),
            actionOnDestroy: (trail) => Destroy(trail.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    public override void Attack()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Time.time < _nextTimeToFire) return;
        _nextTimeToFire = Time.time + _gunData.FireRate;

        _shootSystem.Play();

        //화면 중앙을 발사 방향으로 설정
        Vector3 screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray aimRay = _mainCamera.ScreenPointToRay(screenCenter);
        if (!Physics.Raycast(aimRay, out var aimHitInfo, _gunData.Range, _gunData.HitLayerMask))
        {
            aimHitInfo = new RaycastHit { point = aimRay.origin + aimRay.direction * _gunData.Range };
        }

        //발사 방향에 스프레드 적용
        Vector3 shootDirection = (aimHitInfo.point - _shootSystem.transform.position).normalized;
        shootDirection += Random.insideUnitSphere * _gunData.Spread;
        shootDirection.Normalize();

        //최종 히트 포인트 계산
        Ray shootRay = new(_shootSystem.transform.position, shootDirection);
        if (!Physics.Raycast(shootRay, out var shootHitInfo, _gunData.Range, _gunData.HitLayerMask))
        {
            shootHitInfo = new RaycastHit { point = shootRay.origin + shootRay.direction * _gunData.Range };
        }

        //데미지 처리
        if (shootHitInfo.collider != null && shootHitInfo.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_gunData.Damage);
        }

        //총알 궤적 이펙트 생성
        StartCoroutine(TrailCoroutine(_shootSystem.transform.position, shootHitInfo.point));
    }

    private IEnumerator TrailCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        var trail = _trailPool.Get();
        trail.transform.position = startPosition;
        trail.Clear();
        trail.emitting = true;

        yield return null; // 한 프레임 대기하여 Trail Renderer가 시작 위치에 위치하도록 함

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / _gunData.Speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            trail.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = endPosition;
        trail.emitting = false;
        yield return new WaitForSeconds(trail.time); // Trail이 사라질 때까지 대기
        _trailPool.Release(trail);
    }
}