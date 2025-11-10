using System;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용하는 매니저 클래스
/// 플레이어와 카메라를 초기화
/// 플레이어에게 기본 무기 장착
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private EnemyManager _enemyManager;

    public Player Player { get; private set; }

    private void Start()
    {
        StartGame();
    }

    // 플레이어 생성 및 카메라 타겟 설정
    private void StartGame()
    {
        InitPlayer();
        InitCameraTarget();
        SpawnEnemies();
        InputManager.Instance.EnablePlayerInput();
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        Player = Instantiate(_playerData.PlayerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        Player.Init(_playerData, _weaponData);
    }

    // 시네머신 카메라의 팔로우 타겟 설정
    private void InitCameraTarget()
    {
        var camera = FindFirstObjectByType<CinemachineCamera>();
        if (camera != null)
        {
            Player.SetCameraFollowTarget(camera);
        }
    }

    private void SpawnEnemies()
    {
        _enemyManager.SpawnEnemies(Player.transform, 10);
    }
}