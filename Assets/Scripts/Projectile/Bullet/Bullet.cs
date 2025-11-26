using System;
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

    #region 변수
    public Vector3 SpawnPos { get; private set; } //발사 위치
    public HashSet<Collider> HitColliders { get; private set; } = new(); //충돌한 콜라이더 기록
    private Trail _trail; //총알 궤적 이펙트
    private int _remainPenetrationCount = 0; //남은 관통 횟수
    private int _remainRicochetCount = 0; //남은 튕김 횟수
    #endregion

    #region 이벤트
    public event Action<Bullet, Collision> OnBulletCollision;
    #endregion

    //컴포넌트 가져오기
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //총알 데이터 초기화
    public void Init(BulletData data)
    {
        Data = data;
    }

    #region 발사 및 지연 비활성화
    //총알 발사
    public void Fire(Vector3 initialSpeed, float lifetime, int penetrationCount = 0, int ricochetCount = 0)
    {
        //발사 위치 기록. 총알 적중 시 거리 계산용
        SpawnPos = transform.position;
        _rigidbody.linearVelocity = initialSpeed;

        //남은 관통 및 튕김 횟수 설정
        _remainPenetrationCount = penetrationCount;
        _remainRicochetCount = ricochetCount;

        StartCoroutine(DelayedDisable(lifetime));
    }

    //튕김 시 속도 설정
    public void SetSpeed(Vector3 speed)
    {
        _rigidbody.linearVelocity = speed;
    }

    //지연 후 비활성화
    private IEnumerator DelayedDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnCollisionEnter(null);
    }
    #endregion

    #region 궤적
    //총알 궤적 이펙트 부착
    public void AttachTrail(Trail trail)
    {
        _trail = trail;
        _trail.transform.SetParent(transform, false);
        _trail.transform.localPosition = Vector3.zero;
        _trail.TrailRenderer.Clear();
        _trail.TrailRenderer.emitting = true;
    }

    //총알 궤적 이펙트 분리
    public Trail DetachTrail()
    {
        if (_trail == null) return null;

        _trail.transform.SetParent(null, true);
        _trail.TrailRenderer.emitting = false;
        var tmp = _trail;
        _trail = null;
        return tmp;
    }
    #endregion

    #region 충돌 및 비활성화
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && HitColliders.Contains(collision.collider)) return;
        if (collision != null) HitColliders.Add(collision.collider);
        OnBulletCollision?.Invoke(this, collision);
    }

    //비활성화 시 정지
    private void OnDisable()
    {
        StopAllCoroutines();
        HitColliders.Clear();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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