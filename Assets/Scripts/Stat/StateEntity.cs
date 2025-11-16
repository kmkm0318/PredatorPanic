using System;
using UnityEngine;

/// <summary>
/// 스탯 엔티티 클래스
/// 에디터에서 스탯 타입과 값을 쌍으로 다루기 위한 제너럴 클래스
/// </summary>
[Serializable]
public class StatEntity<T> where T : Enum
{
    [SerializeField] private T _statType;
    [SerializeField] private float _baseValue;
    public T StatType => _statType;
    public float BaseValue => _baseValue;
}