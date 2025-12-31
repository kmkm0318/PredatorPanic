using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region 레퍼런스
    private Enemy _enemy;
    private Transform _target;
    #endregion

    #region 데이터
    private EnemyControllerData _enemyControllerData;
    #endregion

    #region 이동 변수
    private bool _isMoving = false;
    #endregion

    public void Init(Enemy enemy)
    {
        //데이터 할당
        _enemy = enemy;
        _enemyControllerData = enemy.EnemyData.EnemyControllerData;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //이동 중이 아니면 리턴, 타겟이 없으면 리턴
        if (!_isMoving || _target == null) return;

        //타겟과의 거리 계산
        float distanceSqr = (_target.position - transform.position).sqrMagnitude;

        //최소 이동 거리 이내면 리턴
        if (distanceSqr < _enemyControllerData.MinMoveDistanceSqr) return;

        HandleRotation();
        HandleHorizontalMovement();
        HandleVerticalMovement();
    }

    #region 회전
    private void HandleRotation()
    {
        //수평 방향으로만 회전
        var dir = _target.position - transform.position;
        dir.y = 0;

        //방향이 0이면 리턴
        if (dir == Vector3.zero) return;

        //목표 방향으로 부드럽게 회전
        var toTarget = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toTarget, _enemyControllerData.RotationSpeed * Time.deltaTime);
    }
    #endregion

    #region 움직임
    private void HandleHorizontalMovement()
    {
        //속도 가져오기
        float speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;

        //앞으로 이동
        var velocity = transform.forward * speed;

        //이동
        _enemy.transform.position += velocity * Time.deltaTime;
    }

    private void HandleVerticalMovement()
    {
        //지상 이동일 경우
        if (_enemyControllerData.MoveType == EnemyMoveType.Walking)
        {
            // TODO: 현재는 아무것도 하지 않음. 추후 확인하기.

            // //땅 체크
            // bool isGrounded = PhysicsUtility.RaycastNonAlloc(
            //     _enemy.transform.position,
            //     Vector3.down,
            //     _enemyControllerData.GroundCheckDistance,
            //     _enemyControllerData.GroundLayerMask,
            //     out var hitInfo
            // ) > 0;

            // //땅에 닿아있으면 리턴
            // if (isGrounded)
            // {
            //     //땅에 붙이기
            //     _enemy.transform.position = hitInfo[0].point;
            // }
            // else
            // {
            //     //중력 적용
            //     float gravity = _enemyControllerData.Gravity;
            //     var velocity = Vector3.up * gravity;
            //     _enemy.transform.position += velocity * Time.deltaTime;
            // }
        }
        //비행 이동일 경우
        else
        {
            //속도 가져오기
            float speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;

            //높이 차이 계산
            float heightDiff = _target.position.y - _enemy.transform.position.y;

            //위아래로 이동
            var dir = heightDiff > 0 ? Vector3.up : Vector3.down;

            var velocity = dir * speed;
            _enemy.transform.position += velocity * Time.deltaTime;
        }
    }
    #endregion

    #region 타겟 설정
    public void SetTarget(Transform target)
    {
        //타겟 설정
        _target = target;

        //_target 방향 즉시 바라보기
        LookTarget();

        //이동 시작
        _isMoving = true;
    }

    private void LookTarget()
    {
        if (_target == null) return;

        //수평 방향으로만 회전
        var dir = _target.position - transform.position;
        dir.y = 0;

        var toTarget = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = toTarget;
    }
    #endregion
}