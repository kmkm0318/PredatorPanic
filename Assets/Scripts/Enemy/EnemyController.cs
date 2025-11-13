using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// NavMeshAgent 컴포넌트를 활용
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyControllerData _enemyControllerData;
    private Transform _target;
    private NavMeshAgent _navMeshAgent;

    public void Init(Enemy enemy, EnemyControllerData enemyControllerData)
    {
        _enemy = enemy;
        _enemyControllerData = enemyControllerData;

        InitComponents();
    }

    private void InitComponents()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;
    }

    private void Update()
    {
        HandleMovement();
    }

    //적 이동 처리. Update에서 호출하는 것이 최적화에 문제가 없을 것으로 판단.
    private void HandleMovement()
    {
        if (_target != null)
        {
            _navMeshAgent.SetDestination(_target.position);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}