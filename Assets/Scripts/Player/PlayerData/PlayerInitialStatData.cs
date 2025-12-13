using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 초기 스탯 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "PlayerInitialStatData", menuName = "SO/Player/PlayerInitialStatData", order = 0)]
public class PlayerInitialStatData : ScriptableObject
{
    [Header("Initial Stats")]
    [SerializeField] private List<StatEntity<PlayerStatType>> _initialStats;
    public List<StatEntity<PlayerStatType>> InitialStats => _initialStats;
}