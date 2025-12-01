using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행성 주변의 오브젝트들을 행성 표면에 맞게 회전시켜주는 클래스
/// 중력은 작용하지 않음
/// </summary>
public class Planet : MonoBehaviour
{
    //타겟 리스트
    private List<Transform> _targets = new();

    #region 프로퍼티
    public Vector3 Center => transform.position;
    public float Radius => transform.localScale.x * 0.5f;
    #endregion

    private void Update()
    {
        HandleTargetUpRotation();
    }

    //타겟 리스트의 위쪽 방향 설정
    private void HandleTargetUpRotation()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            //타겟 지정
            Transform target = _targets[i];

            //타겟 회전 처리
            RotateTargetToUpRotation(target);
        }
    }

    //타겟이 행성 표면 방향을 위로 향하도록 회전
    private void RotateTargetToUpRotation(Transform target)
    {
        if (target == null) return;

        target.rotation = GetUpRotation(target);
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

    #region 타겟 등록 및 등록 해제
    //타겟 등록
    public void RegisterTarget(Transform target)
    {
        if (!_targets.Contains(target))
        {
            _targets.Add(target);

            //등록 시에 바로 회전 처리
            RotateTargetToUpRotation(target);
        }
    }

    //타겟 등록 해제
    public void UnregisterTarget(Transform target)
    {
        if (_targets.Contains(target))
        {
            _targets.Remove(target);
        }
    }
    #endregion
}