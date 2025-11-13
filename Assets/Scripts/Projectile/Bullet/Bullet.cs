using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 총알 클래스
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    #region 컴포넌트
    private Rigidbody _rigidbody;
    #endregion

    #region 변수
    public Vector3 SpawnPos { get; private set; }
    #endregion

    #region 이벤트
    public event Action<Bullet, Collision> OnBulletCollision;
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //총알 발사
    public void Fire(Vector3 initialSpeed, float lifetime)
    {
        //발사 위치 기록. 총알 적중 시 거리 계산용
        SpawnPos = transform.position;
        _rigidbody.linearVelocity = initialSpeed;

        StartCoroutine(DelayedDisable(lifetime));
    }

    //지연 후 비활성화
    private IEnumerator DelayedDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnCollisionEnter(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnBulletCollision?.Invoke(this, collision);
    }

    //비활성화 시 정지
    private void OnDisable()
    {
        StopAllCoroutines();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}