using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 행성 주변의 오브젝트들을 행성 표면에 맞게 회전시켜주는 클래스
/// 중력은 작용하지 않음
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 15f;

    #region 컴포넌트
    private SphereCollider _collider;
    #endregion

    #region 프로퍼티
    public Vector3 Center => transform.position;
    public float Radius => _collider.radius;
    #endregion

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    //타겟이 행성 표면 방향을 위로 향하도록 회전
    public void RotateTargetToUpRotation(Transform target, bool instant = false)
    {
        if (target == null) return;

        //행성 표면 방향에 맞는 회전값 계산
        var targetRotation = GetUpRotation(target);

        if (instant)
        {
            //즉시 회전
            target.rotation = targetRotation;
        }
        else
        {
            //부드럽게 회전
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
        }
    }

    //타겟의 수직 회전값을 행성 표면에 맞게 계산
    private Quaternion GetUpRotation(Transform target)
    {
        //행성의 중심에서 타겟 방향의 벡터 계산
        Vector3 dir = (target.position - transform.position).normalized;

        //벡터가 0이면 현재 회전값 반환
        if (dir == Vector3.zero) return target.rotation;

        //타겟의 위쪽 방향을 dir로 맞춤
        var rotation = Quaternion.FromToRotation(target.up, dir) * target.rotation;
        return rotation;
    }

    //적이 플레이어를 바라볼 때 행성 표면 방향을 바라보도록 계산
    public Quaternion GetToTargetRotation(Transform agent, Transform target)
    {
        //에이전트의 위쪽 방향. 이때 행성 중앙에서 표면 방향이라고 가정
        var upDir = agent.up;

        //타겟까지의 방향
        var dirToTarget = (target.position - agent.position).normalized;

        //타겟까지의 방향을 행성 표면에 맞게 투영
        var toTarget = Vector3.ProjectOnPlane(dirToTarget, upDir).normalized;

        //lookDir가 0벡터이면 현재 회전값 반환
        if (toTarget == Vector3.zero)
        {
            return agent.rotation;
        }

        //타겟 방향을 바라보는 회전값 반환
        return Quaternion.LookRotation(toTarget, upDir);
    }
}