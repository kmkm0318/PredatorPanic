using System;
using UnityEngine;

/// <summary>
/// 적 스탯 타입과 값의 쌍 클래스
/// </summary>
[Serializable]
public class EnemyStatEntity : IStatEntity<EnemyStatType>
{
    [SerializeField] private EnemyStatType _statType;
    [SerializeField] private float _value;
    public EnemyStatType StatType => _statType;
    public float Value => _value;
}