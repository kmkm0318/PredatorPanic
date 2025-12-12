using UnityEngine;

/// <summary>
/// 효과 데이터 클래스
/// </summary>
public abstract class EffectData : ScriptableObject
{
    [Header("Basic Info")]
    [field: SerializeField, TextArea] public string Description { get; private set; }

    /// <summary>
    /// 효과 생성 메서드
    /// </summary>
    public abstract Effect GetEffect();

    /// <summary>
    /// 효과 설명 반환 메서드
    /// </summary>
    public virtual string GetDescription()
    {
        return Description;
    }
}