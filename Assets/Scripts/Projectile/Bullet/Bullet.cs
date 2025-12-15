using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총알 클래스
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    #region 데이터
    public BulletData Data { get; private set; }
    #endregion

    #region 컴포넌트
    private Rigidbody _rigidbody;
    #endregion

    #region 레퍼런스 변수
    private BulletManager _bulletManager;
    private ExplosionManager _explosionManager;
    private Trail _trail; //총알 궤적 이펙트
    #endregion

    #region 변수
    private BulletFireContext _context;
    private Vector3 _spawnPos; //발사 위치
    public HashSet<Collider> HitColliders { get; private set; } = new(); //충돌한 콜라이더 기록
    private int _remainPenetrationCount = 0; //남은 관통 횟수
    private int _remainRicochetCount = 0; //남은 튕김 횟수
    #endregion

    #region 코루틴
    private Coroutine _lifetimeCoroutine;
    #endregion

    //컴포넌트 가져오기
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //총알 데이터 초기화 및 매니저 레퍼런스 설정
    public void Init(BulletData data, BulletManager bulletManager)
    {
        Data = data;
        _bulletManager = bulletManager;
        _explosionManager = bulletManager.GameManager.ExplosionManager;
    }

    #region 발사 및 지연 비활성화
    //총알 발사
    public void Fire(in BulletFireContext context)
    {
        //컨텍스트 저장
        _context = context;

        //발사 위치 기록. 총알 적중 시 거리 계산용
        _spawnPos = transform.position;

        //남은 관통 및 튕김 횟수 설정
        _remainPenetrationCount = context.PenetrationCount;
        _remainRicochetCount = context.RicochetCount;

        //속도 설정
        _rigidbody.linearVelocity = context.FireDirection * _context.Speed;

        //사거리 기반 생존 시간 후 비활성화
        float lifetime = _context.Range / _context.Speed;
        StartLifetime(lifetime);
    }
    #endregion

    #region 궤적
    //총알 궤적 이펙트 부착
    public void AttachTrail(Trail trail)
    {
        if (trail == null) return;

        _trail = trail;
        trail.AttachToBullet(this);
    }

    //총알 궤적 이펙트 분리
    public void DetachTrail()
    {
        if (_trail == null) return;

        _trail.DetachFromBullet();
        _trail = null;
    }
    #endregion

    #region 폭발
    //폭발 실행 함수
    //폭발 실행은 총알이 튕겨나갈 때, 관통할 때, 또는 반환될 때 호출
    private void ExecuteExplosion()
    {
        if (_context.ExplosionData == null) return;
        if (_explosionManager == null) return;

        //폭발 효과 가져오기
        var explosion = _explosionManager.GetExplosion(_context.ExplosionData);

        //폭발 위치 및 회전 설정
        explosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        //폭발 반경 계산
        var radius = CombatUtility.EXPLOSION_RADIUS_RATIO * _context.Range;

        //폭발 컨텍스트 생성
        ExplosionExplodeContext context = new(_context.Player, _context.Gun, _context.BaseDamage, radius, _context.CriticalRate, _context.CriticalDamageRate, _context.HitLayerMask);

        //폭발 실행
        explosion.Explode(context);
    }
    #endregion

    #region 충돌 및 비활성화
    void OnTriggerEnter(Collider other)
    {
        //비활성화 시 무시
        if (!gameObject.activeSelf) return;

        if (other == null) return;

        if (HitColliders.Contains(other)) return;
        HitColliders.Add(other);

        if (!other.TryGetComponent<Enemy>(out var enemy))
        {
            //땅이나 벽과 충돌한 경우 총알 비활성화
            StopLifetime();
            return;
        }

        //충돌 지점 처리
        var contact = enemy.CenterPosition;

        //거리에 따른 데미지 적용
        float distance = Vector3.Distance(_spawnPos, contact);
        ApplyDamage(enemy, distance);

        //튕김 시도
        if (TryRicochet())
        {
            //가장 가까운 적 찾기. 현재 적 제외
            float halfRange = _context.Range / 2f;
            var targetCollider = PhysicsUtility.GetNearestCollider(contact, halfRange, _context.HitLayerMask, HitColliders);

            //튕길 방향 계산 및 속도 설정
            if (targetCollider != null && targetCollider.TryGetComponent<Enemy>(out var targetEnemy))
            {
                Vector3 center = targetEnemy.CenterPosition;
                var direction = (center - contact).normalized;

                Vector3 newSpeed = direction * _context.Speed;
                _rigidbody.linearVelocity = newSpeed;

                //튕길 때 폭발 실행
                ExecuteExplosion();

                //튕김 성공 시 관통 시도하지 않음
                return;
            }
        }

        //튕김 실패 시 관통 시도
        if (TryPenetrate())
        {
            //관통 성공 시 폭발 실행
            ExecuteExplosion();
        }
        else
        {
            //관통 실패 시 총알 비활성화
            StopLifetime();
        }
    }

    private void ApplyDamage(Enemy enemy, float distance)
    {
        //거리 비례 데미지 계산
        float damage = CombatUtility.CalculateRangedDamage(_context.BaseDamage, _context.Range, distance);

        //치명타 여부 결정
        bool isCritical = Random.value < _context.CriticalRate;

        //치명타 데미지 적용
        if (isCritical)
        {
            damage *= _context.CriticalDamageRate;
        }

        //데미지 컨텍스트 생성
        PlayerDamageContext context = new(_context.Player, _context.Gun, enemy, damage, isCritical);

        //데미지 적용
        enemy.TakeDamage(context);
    }

    private void ReleaseBullet()
    {
        //궤적 분리
        DetachTrail();

        //폭발 실행
        ExecuteExplosion();

        //변수들 초기화
        HitColliders.Clear();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        //풀에 반환
        _bulletManager.ReleaseBullet(this);
    }
    #endregion

    #region 관통 및 튕김 시도
    public bool TryPenetrate()
    {
        if (_remainPenetrationCount > 0)
        {
            _remainPenetrationCount--;
            return true;
        }
        return false;
    }

    public bool TryRicochet()
    {
        if (_remainRicochetCount > 0)
        {
            _remainRicochetCount--;
            return true;
        }
        return false;
    }
    #endregion

    #region 라이프타임 코루틴
    private IEnumerator LifetimeCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopLifetime();
    }

    private void StartLifetime(float duration)
    {
        StopLifetime();
        _lifetimeCoroutine = StartCoroutine(LifetimeCoroutine(duration));
    }

    private void StopLifetime()
    {
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
            ReleaseBullet();
        }
    }
    #endregion
}