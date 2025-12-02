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
    private Gun _gun; //발사한 총기
    private Trail _trail; //총알 궤적 이펙트
    #endregion

    #region 변수
    public Vector3 SpawnPos { get; private set; } //발사 위치
    private LayerMask _hitLayerMask; //충돌 레이어 마스크   
    public HashSet<Collider> HitColliders { get; private set; } = new(); //충돌한 콜라이더 기록
    private int _remainPenetrationCount = 0; //남은 관통 횟수
    private int _remainRicochetCount = 0; //남은 튕김 횟수
    private float _baseDamage;
    private float _speed;
    private float _range;
    private float _criticalRate;
    private float _criticalDamageRate;
    #endregion

    //컴포넌트 가져오기
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //총알 데이터 초기화
    public void Init(BulletData data, BulletManager bulletManager)
    {
        Data = data;
        _bulletManager = bulletManager;
    }

    #region 발사 및 지연 비활성화
    //총알 발사
    public void Fire(BulletFireContext context)
    {
        //발사 위치 기록. 총알 적중 시 거리 계산용
        SpawnPos = transform.position;

        //발사한 총기 설정
        _gun = context.Gun;

        //데미지, 속도, 사거리 설정
        _baseDamage = context.BaseDamage;
        _speed = context.Speed;
        _range = context.Range;

        //크리티컬 확률 및 크리티컬 데미지 설정
        _criticalRate = context.CriticalRate;
        _criticalDamageRate = context.CriticalDamageRate;

        //남은 관통 및 튕김 횟수 설정
        _remainPenetrationCount = context.PenetrationCount;
        _remainRicochetCount = context.RicochetCount;

        //충돌 레이어 마스크 설정
        _hitLayerMask = context.HitLayerMask;

        //속도 설정
        _rigidbody.linearVelocity = context.FireDirection * _speed;

        //사거리 기반 생존 시간 후 비활성화
        float lifetime = _range / _speed;
        StartCoroutine(DelayedDisable(lifetime));
    }

    //지연 후 비활성화  
    private IEnumerator DelayedDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseBullet();
    }
    #endregion

    #region 궤적
    //총알 궤적 이펙트 부착
    public void AttachTrail(Trail trail)
    {
        if (trail == null) return;

        _trail = trail;
        trail.AttachOnBullet(this);
    }

    //총알 궤적 이펙트 분리
    public void DetachTrail()
    {
        if (_trail == null) return;

        _trail.DetachFromBullet();
        _trail = null;
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
            //땅이나 벽과 충돌한 경우
            ReleaseBullet();
            return;
        }

        //충돌 지점 처리
        var contact = other.ClosestPoint(transform.position);

        //거리에 따른 데미지 적용
        float distance = Vector3.Distance(SpawnPos, contact);
        ApplyDamage(enemy, distance);

        //튕김 시도
        if (TryRicochet())
        {
            //가장 가까운 적 찾기. 현재 적 제외
            float halfRange = _range / 2f;
            var targetCollider = CombatUtility.GetNearestCollider(contact, halfRange, _hitLayerMask, HitColliders);

            //튕길 방향 계산 및 속도 설정
            if (targetCollider != null)
            {
                Vector3 center = targetCollider.bounds.center;
                var direction = (center - contact).normalized;

                Vector3 newSpeed = direction * _speed;
                _rigidbody.linearVelocity = newSpeed;

                //튕김 성공 시 관통 시도하지 않음
                return;
            }
        }

        //튕김 실패 시 관통 시도
        //관통 실패 시 총알 해제
        if (!TryPenetrate())
        {
            ReleaseBullet();
        }
    }

    private void ApplyDamage(Enemy enemy, float distance)
    {
        //거리 비례 데미지 계산
        float damage = CombatUtility.CalculateBulletFinalDamage(_baseDamage, _range, distance);

        //치명타 여부 결정
        bool isCritical = UnityEngine.Random.value < _criticalRate;
        if (isCritical)
        {
            damage *= _criticalDamageRate;
        }

        //데미지 컨텍스트 생성
        PlayerDamageContext context = new()
        {
            Player = _gun.Player,
            Weapon = _gun,
            Enemy = null, //적 정보는 IDamageable에서 처리
            Damage = damage, //방어력을 적용하기 전 데미지
            IsCritical = isCritical,
        };

        //데미지 적용
        enemy.TakeDamage(context);
    }

    private void ReleaseBullet()
    {
        //궤적 분리
        DetachTrail();

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
}