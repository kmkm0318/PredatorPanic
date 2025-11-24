using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 비주얼 컴포넌트
/// 카메라 피벗과 무기 피벗 포함
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    #region 무기 피벗
    [SerializeField] private float _weaponPivotHeight = 1f;
    [SerializeField] private float _weaponPivotRadius = 1f;
    private Transform _weaponPivotContainer;
    private List<Transform> _weaponPivots = new();
    public List<Transform> WeaponPivots => _weaponPivots;
    #endregion

    #region 카메라 피벗
    [SerializeField] private Transform _cameraPivot;
    public Transform CameraPivot => _cameraPivot;
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

        InitWeaponPivotContainer();
    }

    //무기 피벗 초기화
    private void InitWeaponPivotContainer()
    {
        // 무기 피벗 컨테이너 생성
        _weaponPivotContainer = new GameObject("WeaponPivotContainer").transform;
        _weaponPivotContainer.SetParent(transform);
        _weaponPivotContainer.SetLocalPositionAndRotation(Vector3.up * _weaponPivotHeight, Quaternion.identity);
    }

    //새로운 무기 피벗 생성 및 반환
    public Transform GetNewWeaponPivot()
    {
        Transform pivot = new GameObject($"WeaponPivot_{_weaponPivots.Count}").transform;
        pivot.SetParent(_weaponPivotContainer);
        pivot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        _weaponPivots.Add(pivot);

        UpdatePivotPositions();

        return pivot;
    }

    //인덱스로 무기 피벗 제거. 무기 오브젝트도 함께 제거
    public void RemoveWeaponPivot(int idx)
    {
        if (idx < 0 || idx >= _weaponPivots.Count) return;
        Transform pivot = _weaponPivots[idx];
        if (_weaponPivots.Remove(pivot))
        {
            Destroy(pivot.gameObject);
            UpdatePivotPositions();
        }
    }

    //무기 피벗 위치를 플레이어 상단에 반원으로 배치
    private void UpdatePivotPositions()
    {
        int count = _weaponPivots.Count;

        if (count == 0) return;

        if (count == 1)
        {
            //하나일 때는 우측에 배치
            Transform pivot = _weaponPivots[0];
            pivot.SetLocalPositionAndRotation(new Vector3(_weaponPivotRadius, 0f, 0f), Quaternion.identity);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            //처음 시작은 우측, 끝은 좌측
            float angle = Mathf.PI * i / (count - 1);
            float x = Mathf.Cos(angle) * _weaponPivotRadius;
            float y = Mathf.Sin(angle) * _weaponPivotRadius;

            //각 피벗 위치와 각도 초기화
            Transform pivot = _weaponPivots[i];
            pivot.SetLocalPositionAndRotation(new Vector3(x, y, 0f), Quaternion.identity);
        }
    }
}