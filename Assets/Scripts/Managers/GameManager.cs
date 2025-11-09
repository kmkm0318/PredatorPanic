using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용하는 매니저 클래스
/// 플레이어와 카메라를 초기화
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _playerSpawnPoint;

    private Player _player;

    private void Start()
    {
        StartGame();
    }

    // 플레이어 생성 및 카메라 타겟 설정
    private void StartGame()
    {
        InitPlayer();
        InitCameraTarget();
        InputManager.Instance.EnablePlayerInput();
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        _player = Instantiate(_playerData.PlayerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _player.Init(_playerData);
    }

    // 시네머신 카메라의 팔로우 타겟 설정
    private void InitCameraTarget()
    {
        var camera = FindFirstObjectByType<CinemachineCamera>();
        if (camera != null)
        {
            camera.Follow = _player.CameraPivot;
        }
    }
}
