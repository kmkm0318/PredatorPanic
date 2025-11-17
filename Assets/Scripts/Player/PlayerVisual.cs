using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 비주얼 컴포넌트
/// 카메라 피벗과 무기 피벗 포함
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    #region 카메라, 무기 피벗
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private List<Transform> _weaponPivots = new();
    public Transform CameraPivot => _cameraPivot;
    public List<Transform> WeaponPivots => _weaponPivots;
    #endregion

    #region 컴포넌트
    public Animator Animator { get; private set; }
    #endregion

    #region 애니메이션 이름, 해시
    private const string IS_MOVING = "isMoving";
    private const string IS_JUMPING = "isJumping";
    private const string IS_FALLING = "isFalling";
    public int IsMovingHash { get; private set; } = Animator.StringToHash(IS_MOVING);
    public int IsJumpingHash { get; private set; } = Animator.StringToHash(IS_JUMPING);
    public int IsFallingHash { get; private set; } = Animator.StringToHash(IS_FALLING);
    #endregion

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public Transform GetWeaponPivot(int idx)
    {
        if (idx < 0 || idx >= _weaponPivots.Count)
        {
            Debug.LogWarning("무기 피벗 인덱스 범위 초과");
            return null;
        }

        return _weaponPivots[idx];
    }
}