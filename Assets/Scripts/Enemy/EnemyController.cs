using UnityEngine;
using UnityEngine.AI;

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
    }

    private void Update()
    {
        HandleMovement();
    }

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