using UnityEngine;

/// <summary>
/// 아이템 효과 데이터 클래스
/// </summary>
public abstract class ItemEffectData : ScriptableObject
{
    [Header("Effect Info")]
    [field: SerializeField, TextArea] public string Description { get; private set; }

    /// <summary>
    /// 아이템 효과를 생성하고 반환합니다.
    /// </summary>
    public abstract ItemEffect GetItemEffect();
}
