using System;
using UnityEngine;

/// <summary>
/// 스탯 엔티티 클래스
/// 에디터에서 스탯 타입과 값을 쌍으로 다루기 위한 제너럴 클래스
/// 기본값을 정의하는 데에 사용합니다.
/// 적의 레벨 당 스탯 증가량을 위해서도 사용합니다.
/// </summary>
[Serializable]
public class StatEntity<T> where T : Enum
{
    [SerializeField] private T _statType;
    [SerializeField] private float _value;
    public T StatType => _statType;
    public float Value => _value;
}