using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 스크립터블 오브젝트
/// 플레이어 프리팹, 비주얼 프리팹, 컨트롤러 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    [SerializeField] private Player _playerPrefab;
    public Player PlayerPrefab => _playerPrefab;

    [Header("Player Controller Data")]
    [SerializeField] private PlayerControllerData _playerControllerData;
    public PlayerControllerData PlayerControllerData => _playerControllerData;

    [Header("Stats")]
    [SerializeField] private PlayerInitialStatData _playerInitialStatData;
    public PlayerInitialStatData PlayerInitialStatData => _playerInitialStatData;

    [Header("EXP")]
    [SerializeField] private PlayerExpData _playerExpData;
    public PlayerExpData PlayerExpData => _playerExpData;

    [Header("Weapon")]
    [SerializeField] private int _weaponCountMax = 6;
    public int WeaponCountMax => _weaponCountMax;

    [Header("Item Pickup")]
    [SerializeField] private float _itemPickupRadiusSqr = 4f;
    public float ItemPickupRadiusSqr => _itemPickupRadiusSqr;
}