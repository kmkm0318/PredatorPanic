using System;
using UnityEngine;

/// <summary>
/// 플레이어 스탯 타입과 값의 쌍 클래스
/// </summary>
[Serializable]
public class PlayerStatEntity : IStatEntity<PlayerStatType>
{
    [SerializeField] private PlayerStatType _statType;
    [SerializeField] private float _value;
    public PlayerStatType StatType => _statType;
    public float Value => _value;
}