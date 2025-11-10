using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    private EnemyControllerData _enemyControllerData;
    private Transform _target;
    private CharacterController _characterController;

    private Vector3 _movement;

    public void Init(EnemyControllerData enemyControllerData)
    {
        _enemyControllerData = enemyControllerData;

        InitComponents();
    }

    private void InitComponents()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleMovement();
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            _movement.y = _enemyControllerData.GroundedGravitySpeed;
        }
        else
        {
            _movement.y += _enemyControllerData.Gravity * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        if (_target != null)
        {
            Vector3 directionToTarget = _target.position - transform.position;
            directionToTarget.y = 0;

            if (directionToTarget.magnitude < 0.1f) return;
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    private void HandleMovement()
    {
        Vector3 moveVal = transform.forward * _enemyControllerData.MoveSpeed + Vector3.up * _movement.y;
        _characterController.Move(moveVal * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}