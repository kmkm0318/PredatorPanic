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
    [SerializeField] private ParticleSystem _muzzleFlash;
    private GunData _gunData;
    private Camera _mainCamera;
    private ObjectPool<TrailRenderer> _trailPool;
    private float _currentSpread;
    private Coroutine _fireCoroutine;

    public override void Init(WeaponData weaponData)
    {
        base.Init(weaponData);

        _gunData = weaponData as GunData;
        _currentSpread = _gunData.SpreadMin;
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

    private void Update()
    {
        HandleSpread();
    }

    #region 스프레드
    //스프레드 처리
    private void HandleSpread()
    {
        if (IsAttacking)
        {
            IncreaseSpread();
        }
        else
        {
            DecreaseSpread();
        }
    }

    //스프레드 증가
    private void IncreaseSpread()
    {
        _currentSpread += _gunData.SpreadIncreaseRate * Time.deltaTime;
        if (_currentSpread > _gunData.SpreadMax)
        {
            _currentSpread = _gunData.SpreadMax;
        }
    }

    //스프레드 감소
    private void DecreaseSpread()
    {
        if (_currentSpread > _gunData.SpreadMin)
        {
            _currentSpread -= _gunData.SpreadDecreaseRate * Time.deltaTime;
            if (_currentSpread < _gunData.SpreadMin) _currentSpread = _gunData.SpreadMin;
        }
    }
    #endregion

    public override void StartAttack()
    {
        base.StartAttack();
        StartFireCoroutine();
    }

    public override void StopAttack()
    {
        base.StopAttack();
        StopFireCoroutine();
    }

    #region Fire Coroutine
    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(_gunData.FireRate);
        }
    }

    private void StartFireCoroutine()
    {
        StopFireCoroutine();
        _fireCoroutine = StartCoroutine(FireCoroutine());
    }

    private void StopFireCoroutine()
    {
        if (_fireCoroutine != null)
        {
            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }
    }
    #endregion

    private void Fire()
    {
        _muzzleFlash.Play();

        //화면 중앙을 발사 방향으로 설정
        Vector3 screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray aimRay = _mainCamera.ScreenPointToRay(screenCenter);
        if (!Physics.Raycast(aimRay, out var aimHitInfo, _gunData.Range, _gunData.HitLayerMask))
        {
            aimHitInfo = new RaycastHit { point = aimRay.origin + aimRay.direction * _gunData.Range };
        }

        //발사 방향에 스프레드 적용
        Vector3 fireDirection = (aimHitInfo.point - _muzzleFlash.transform.position).normalized;
        fireDirection += Random.insideUnitSphere * _currentSpread;
        fireDirection.Normalize();

        //최종 히트 포인트 계산
        Ray fireRay = new(_muzzleFlash.transform.position, fireDirection);
        if (!Physics.Raycast(fireRay, out var fireHitInfo, _gunData.Range, _gunData.HitLayerMask))
        {
            fireHitInfo = new RaycastHit { point = fireRay.origin + fireRay.direction * _gunData.Range };
        }

        //데미지 처리
        if (fireHitInfo.collider != null && fireHitInfo.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_gunData.Damage);
        }

        //총알 궤적 이펙트 생성
        StartCoroutine(TrailCoroutine(_muzzleFlash.transform.position, fireHitInfo.point));
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