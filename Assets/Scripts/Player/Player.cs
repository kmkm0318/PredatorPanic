using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 플레이어 클래스
/// 플레이어 데이터와 컴포넌트 초기화 담당
/// 각 컴포넌트 간의 연결 관리
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAttack))]
public class Player : MonoBehaviour
{
    #region 플레이어 데이터
    private PlayerData _playerData;
    #endregion

    #region 컴포넌트
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;
    #endregion

    #region 플레이어 비주얼 객체
    private PlayerVisual _playerVisual;
    #endregion

    public void Init(PlayerData playerData, WeaponData weaponData)
    {
        _playerData = playerData;
        InitPlayerVisual();
        InitComponents();
        InitWeapon(weaponData);
    }

    // 플레이어 비주얼 초기화
    private void InitPlayerVisual()
    {
        if (_playerData.PlayerVisualPrefab != null)
        {
            _playerVisual = Instantiate(_playerData.PlayerVisualPrefab, transform);
            _playerVisual.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    // 컴포넌트 초기화
    private void InitComponents()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.Init(this, _playerData.PlayerControllerData, _playerVisual);

        _playerAttack = GetComponent<PlayerAttack>();
    }

    // 무기 초기화
    private void InitWeapon(WeaponData weaponData)
    {
        if (weaponData.WeaponPrefab != null)
        {
            var weapon = Instantiate(weaponData.WeaponPrefab, _playerVisual.WeaponPivot);
            weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            weapon.Init(weaponData);
            _playerAttack.SetWeapon(weapon);
        }
    }

    // 시네머신 카메라의 팔로우 타겟 설정
    public void SetCameraFollowTarget(CinemachineCamera camera)
    {
        if (camera != null)
        {
            camera.Follow = _playerVisual.CameraPivot;
        }
    }

    // 공격 시작, 종료
    public void StartAttack()
    {
        _playerAttack.StartAttack();
    }

    public void StopAttack()
    {
        _playerAttack.StopAttack();
    }
}