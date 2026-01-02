using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총알 클래스
/// </summary>
public class Bullet : MonoBehaviour, IManualUpdate
{
    #region 상수
    private const int DEFAULT_HIT_BUFFER_SIZE = 16;
    private const float RAYCAST_START_OFFSET = 0.1f;
    private const float MIN_HOMING_DISTANCE_SQR = 0.01f;
    #endregion

    #region 데이터
    public BulletData Data { get; private set; }
    #endregion

    #region 레퍼런스 변수
    private BulletManager _bulletManager;
    private ExplosionManager _explosionManager;
    private Transform _transform;
    #endregion

    #region 컨텍스트
    private BulletFireContext _context;
    #endregion

    #region 이동
    private Vector3 _direction;
    private float _speed;
    public HashSet<Collider> HitColliders { get; private set; } = new(DEFAULT_HIT_BUFFER_SIZE); //충돌한 콜라이더 기록
    #endregion

    #region 호밍
    private bool _isHoming = false;
    private Enemy _targetEnemy = null;
    private float _homingDelayTimer = 0f;
    #endregion

    #region 관통, 튕김
    private int _remainPenetrationCount = 0; //남은 관통 횟수
    private int _remainRicochetCount = 0; //남은 튕김 횟수
    #endregion

    #region 라이프타임
    public bool IsActive { get; set; } = false;
    private float _lifetimeElapsed = 0f;
    private float _lifetimeDuration = 0f;
    #endregion

    private void Awake()
    {
        //트랜스폼 캐싱
        _transform = transform;
    }

    #region 초기화
    //총알 데이터 초기화 및 매니저 레퍼런스 설정
    public void Init(BulletData data, BulletManager bulletManager)
    {
        Data = data;
        _bulletManager = bulletManager;
        _explosionManager = bulletManager.GameManager.ExplosionManager;
    }
    #endregion

    /// <summary>
    /// 업데이트 처리
    /// Manager에서 호출해서 처리
    /// </summary>
    public void ManualUpdate(float deltaTime)
    {
        //비활성화 시 패스
        if (!gameObject.activeSelf) return;

        HandleLifetime(deltaTime);

        //라이프 타임 중지 시 패스
        if (!IsActive) return;

        HandleHoming(deltaTime);
        HandleMovement(deltaTime);
    }

    #region 호밍
    private void HandleHoming(float deltaTime)
    {
        //호밍이 아니면 패스
        if (!_isHoming) return;

        //호밍 딜레이 시간 경과 전이면
        if (_homingDelayTimer < Data.HomingDelay)
        {
            //시간 갱신 후 패스
            _homingDelayTimer += deltaTime;
            return;
        }

        //타겟이 없거나 비활성화 되었으면
        if (_targetEnemy == null || !_targetEnemy.gameObject.activeSelf)
        {
            //라이프 타임 내에 도달할 수 있는 최대 거리 계산
            var range = _speed * (_lifetimeDuration - _lifetimeElapsed);

            //가까운 적 찾기
            var targetCollider = PhysicsUtility.GetNearestCollider(_transform.position, range, _context.HitLayerMask, HitColliders);

            //타겟이 있으면
            if (targetCollider != null && targetCollider.TryGetComponent<Enemy>(out var targetEnemy))
            {
                //타겟 설정
                _targetEnemy = targetEnemy;
            }
            //타겟이 없으면
            else
            {
                //호밍 중지
                _isHoming = false;
                return;
            }
        }

        //타겟 방향 계산
        Vector3 toTarget = _targetEnemy.CenterPosition - _transform.position;

        //타겟과의 거리가 너무 가까우면 패스
        if (toTarget.sqrMagnitude <= MIN_HOMING_DISTANCE_SQR) return;

        //호밍 방향 보간
        _direction = Vector3.RotateTowards(_direction, toTarget.normalized, Data.HomingPower * deltaTime, 0f);
    }
    #endregion

    #region 이동
    private void HandleMovement(float deltaTime)
    {
        //이동 거리 계산
        var distance = _speed * deltaTime;

        //레이캐스트 시작 위치 계산
        var raycastOrigin = _transform.position - _direction * RAYCAST_START_OFFSET;

        //레이캐스트 거리 계산
        var rayDistance = distance + RAYCAST_START_OFFSET;

        //레이캐스트로 충돌 판단
        var hitCount = PhysicsUtility.RaycastNonAlloc(raycastOrigin, _direction, rayDistance, _context.HitLayerMask, out var hits, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            //충돌 처리
            HandleHitCollider(hits[i].collider);

            //총알이 비활성화 되었으면 종료
            if (!gameObject.activeSelf) return;
        }

        //위치 갱신
        _transform.position += _direction * distance;
    }
    #endregion

    #region 발사
    //총알 발사
    public void Fire(in BulletFireContext context)
    {
        //컨텍스트 저장
        _context = context;

        //변수 설정
        _direction = context.FireDirection.normalized;
        _speed = context.Speed;

        //호밍 설정
        _targetEnemy = context.InitialTargetEnemy;
        _isHoming = Data.IsHoming;
        _homingDelayTimer = 0f;

        //위치 설정
        _transform.position = context.FirePosition;

        //궤적 이펙트 부착
        AttachTrail();

        //남은 관통 및 튕김 횟수 설정
        _remainPenetrationCount = context.PenetrationCount;
        _remainRicochetCount = context.RicochetCount;

        //사거리 기반 생존 시간 후 비활성화
        float lifetime = _context.Range / _context.Speed;

        StartLifetime(lifetime);
    }
    #endregion

    #region 궤적
    //총알 궤적 이펙트 부착
    private void AttachTrail()
    {
        if (_context.Trail == null) return;

        _context.Trail.AttachToBullet(this);
    }

    //총알 궤적 이펙트 분리
    private void DetachTrail()
    {
        if (_context.Trail == null) return;

        _context.Trail.DetachFromBullet();
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

        //폭발 반경 계산
        var radius = CombatUtility.EXPLOSION_RADIUS_RATIO * _context.Range;

        //폭발 컨텍스트 생성
        ExplosionExplodeContext context = new(
            _context.Player,
            _context.Gun,
            _transform.position,
            _context.BaseDamage,
            radius,
            _context.CriticalRate,
            _context.CriticalDamageRate,
            _context.HitLayerMask
        );

        //폭발 실행
        explosion.Explode(context);
    }
    #endregion

    #region 충돌 및 비활성화
    private void HandleHitCollider(Collider other)
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

        //데미지 적용
        ApplyDamage(enemy);

        //튕김 시도
        if (TryRicochet())
        {
            //라이프 타임 내에 도달할 수 있는 최대 거리 계산
            float range = _context.Range * _lifetimeElapsed / _lifetimeDuration;

            //가장 가까운 적 찾기. 단, 이미 충돌한 적은 제외
            var targetCollider = PhysicsUtility.GetNearestCollider(contact, range, _context.HitLayerMask, HitColliders);

            //튕길 방향 계산 및 속도 설정
            if (targetCollider != null && targetCollider.TryGetComponent<Enemy>(out var targetEnemy))
            {
                Vector3 center = targetEnemy.CenterPosition;
                var newDirection = (center - contact).normalized;

                //방향 갱신
                _direction = newDirection;

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

    private void ApplyDamage(Enemy enemy)
    {
        //데미지 가져오기
        float damage = _context.BaseDamage;

        //치명타 여부 결정
        bool isCritical = _context.CriticalRate.ChanceTest();

        //치명타 데미지 적용
        if (isCritical)
        {
            damage *= _context.CriticalDamageRate;
        }

        //데미지 출처 타입 결정
        //총이 없을 경우 Effect 타입으로 설정
        var damageSourceType = _context.Gun == null ? PlayerDamageSourceType.Effect : PlayerDamageSourceType.Bullet;

        //데미지 컨텍스트 생성
        PlayerDamageContext context = new(
            _context.Player,
            _context.Gun,
            enemy,
            damage,
            isCritical,
            damageSourceType
        );

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

    #region 라이프타임
    private void HandleLifetime(float deltaTime)
    {
        // 비활성화 시 패스
        if (!gameObject.activeSelf) return;

        //라이프타임이 동작 중이지 않으면 패스
        if (!IsActive) return;

        //경과 시간 갱신
        _lifetimeElapsed += deltaTime;

        //지속 시간 경과 시
        if (_lifetimeElapsed >= _lifetimeDuration)
        {
            //비활성화
            StopLifetime();
        }
    }

    private void StartLifetime(float duration)
    {
        //기존 라이프타임 중지
        StopLifetime();

        //활성화
        IsActive = true;

        //변수 설정
        _lifetimeDuration = duration;
        _lifetimeElapsed = 0f;
    }

    private void StopLifetime()
    {
        //동작 중이지 않으면 패스
        if (!IsActive) return;

        //비활성화
        IsActive = false;

        //총알 반환
        ReleaseBullet();
    }
    #endregion
}