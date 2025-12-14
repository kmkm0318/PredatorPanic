using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 기본 스탯 데이터
/// </summary>
[CreateAssetMenu(fileName = "PlayerBaseStatsData", menuName = "SO/Player/PlayerBaseStatsData", order = 0)]
public class PlayerBaseStatsData : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private List<StatEntity<PlayerStatType>> _baseStats;
    public List<StatEntity<PlayerStatType>> BaseStats => _baseStats;
}