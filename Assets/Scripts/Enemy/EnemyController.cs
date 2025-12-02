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

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(Enemy enemy, EnemyControllerData enemyControllerData)
    {
        //데이터 할당
        _enemy = enemy;
        _enemyControllerData = enemyControllerData;

        //스탯 가져오기
        var stat = _enemy.EnemyStats;
        var moveSpeedStat = stat.GetStat(EnemyStatType.MoveSpeed);

        //초기 이동속도 지정
        _navMeshAgent.speed = moveSpeedStat.FinalValue;

        //이동속도 변경 이벤트 등록
        //초기화 시 새로운 Stat이 할당되기 때문에 해제 불필요
        moveSpeedStat.OnValueChanged += (speed) => _navMeshAgent.speed = speed;
    }

    //적 방향 처리. 타겟을 향해 회전
    private IEnumerator HandleMovementCoroutine()
    {
        var wait = new WaitForSeconds(_enemyControllerData.PathUpdateRate);
        while (true)
        {
            if (_target != null)
            {
                //타겟이 있으면 타겟 위치로 이동
                _navMeshAgent.SetDestination(_target.position);
            }
            else
            {
                //타겟이 없으면 정지
                _navMeshAgent.SetDestination(transform.position);
            }

            //대기
            yield return wait;
        }
    }

    //타겟을 지정하는 함수
    public void SetTarget(Transform target)
    {
        _target = target;

        //_target 방향 즉시 바라보기
        LookTarget();

        //이동 코루틴 시작
        StartCoroutine(HandleMovementCoroutine());
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