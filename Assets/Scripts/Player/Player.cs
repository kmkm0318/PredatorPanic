using UnityEngine;

/// <summary>
/// 플레이어 클래스
/// 플레이어 데이터와 컴포넌트 초기화 담당
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    #region 카메라 피벗
    [SerializeField] private Transform _cameraPivot;
    public Transform CameraPivot => _cameraPivot;
    #endregion

    #region 플레이어 데이터
    public PlayerData PlayerData { get; private set; }
    #endregion

    #region 컴포넌트
    public PlayerController PlayerController { get; private set; }
    #endregion

    #region 플레이어 비주얼 객체
    private Transform _playerVisual;
    #endregion

    public void Init(PlayerData playerData)
    {
        PlayerData = playerData;
        InitPlayerVisual();
        InitComponents();
    }

    // 플레이어 비주얼 초기화
    private void InitPlayerVisual()
    {
        if (PlayerData.PlayerVisualPrefab != null)
        {
            _playerVisual = Instantiate(PlayerData.PlayerVisualPrefab, transform);
            _playerVisual.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    // 컴포넌트 초기화
    private void InitComponents()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerController.Init(PlayerData.PlayerControllerData, _cameraPivot, _playerVisual);
    }
}