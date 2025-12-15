using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    #region 상수
    //경로 업데이트 간격
    private const float PATH_UPDATE_INTERVAL = 0.1f;
    #endregion
    #region 레퍼런스
    private Enemy _enemy;
    private Transform _target;
    #endregion

    #region 데이터
    private EnemyControllerData _enemyControllerData;
    #endregion

    #region 컴포넌트
    private NavMeshAgent _navMeshAgent;
    #endregion

    #region 타이머
    private float _nextTimeToUpdatePath = 0f;
    private bool _isMoving = false;
    #endregion

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(Enemy enemy)
    {
        //데이터 할당
        _enemy = enemy;
        _enemyControllerData = enemy.EnemyData.EnemyControllerData;

        //스탯 가져오기
        var stat = _enemy.EnemyStats;
        var moveSpeedStat = stat.GetStat(EnemyStatType.MoveSpeed);

        //초기 이동속도 지정
        _navMeshAgent.speed = moveSpeedStat.FinalValue;

        //이동속도 변경 이벤트 등록
        //초기화 시 새로운 Stat이 할당되기 때문에 해제 불필요
        moveSpeedStat.OnValueChanged += (speed) => _navMeshAgent.speed = speed;
    }

    private void Update()
    {
        HandleUpdatePath();
    }

    //경로 업데이트 처리
    private void HandleUpdatePath()
    {
        //이동 중이 아닐 시 패스
        if (!_isMoving) return;

        //시간이 되지 않았을 시 패스
        if (Time.time < _nextTimeToUpdatePath) return;

        //다음 업데이트 시간 계산
        _nextTimeToUpdatePath = Time.time + PATH_UPDATE_INTERVAL;

        //타겟이 없을 시 이동 중지 및 패스
        if (_target == null)
        {
            _isMoving = false;
            return;
        }

        //네비메시 에이전트 목적지 설정
        var targetPos = _target.position;

        //NavMeshAgent를 사용하기 때문에 수평면으로만 이동
        targetPos.y = transform.position.y;

        _navMeshAgent.SetDestination(targetPos);
    }

    //타겟을 지정하는 함수
    public void SetTarget(Transform target)
    {
        //타겟 설정
        _target = target;

        //_target 방향 즉시 바라보기
        LookTarget();

        //이동 시작
        _isMoving = true;

        //타이머 초기화
        _nextTimeToUpdatePath = 0f;
    }

    //타겟 즉시 바라보기
    private void LookTarget()
    {
        if (_target == null) return;

        //수평 방향으로만 회전
        var dir = _target.position - transform.position;
        dir.y = 0;

        var toTarget = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = toTarget;
    }
}