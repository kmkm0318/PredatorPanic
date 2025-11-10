using UnityEngine;

/// <summary>
/// 플레이어 데이터 스크립터블 오브젝트
/// 플레이어 프리팹, 비주얼 프리팹, 컨트롤러 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    [SerializeField] private Player _playerPrefab;
    public Player PlayerPrefab => _playerPrefab;

    [Header("Player Visual Prefab")]
    [SerializeField] private PlayerVisual _playerVisualPrefab;
    public PlayerVisual PlayerVisualPrefab => _playerVisualPrefab;

    [Header("Player Controller Data")]
    [SerializeField] private PlayerControllerData _playerControllerData;
    public PlayerControllerData PlayerControllerData => _playerControllerData;
}