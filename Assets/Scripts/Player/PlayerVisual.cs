using UnityEngine;

/// <summary>
/// 플레이어 비주얼 컴포넌트
/// 카메라 피벗과 무기 피벗 포함
/// </summary>
public class PlayerVisual : MonoBehaviour
{
    #region 카메라, 무기 피벗
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private Transform _weaponPivot;
    public Transform CameraPivot => _cameraPivot;
    public Transform WeaponPivot => _weaponPivot;
    #endregion
}