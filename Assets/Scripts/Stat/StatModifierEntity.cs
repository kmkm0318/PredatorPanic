using System;
using UnityEngine;

/// <summary>
/// 스탯 모디파이어 엔티티 클래스
/// T는 스탯 타입 Enum. 예를 들어 PlayerStatType, GunStatType
/// 이 클래스는 플레이어 장비, 총기 부착물과 같이 스탯을 변경하는 아이템 등에 사용
/// </summary>
[Serializable]
public class StatModifierEntity<T> where T : Enum
{
    [SerializeField] private T _statType;
    [SerializeField] private float _value;
    [SerializeField] private StatModifierType _type;

    public T StatType => _statType;
    public float Value => _value;
    public StatModifierType Type => _type;
}