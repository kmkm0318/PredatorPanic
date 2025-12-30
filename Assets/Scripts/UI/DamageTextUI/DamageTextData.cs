using DG.Tweening;
using UnityEngine;

/// <summary>
/// 데미지 텍스트를 위한 데이터 컨테이너 클래스
/// </summary>
[CreateAssetMenu(fileName = "DamageTextData", menuName = "SO/UI/DamageText/DamageTextData", order = 0)]
public class DamageTextData : ScriptableObject
{
    [Header("Prefab")]
    [field: SerializeField] public DamageText DamageTextPrefab { get; private set; }

    [Header("Type")]
    [field: SerializeField] public DamageTextType DamageTextType { get; private set; } = DamageTextType.Normal;

    [Header("Animation Settings")]
    [field: SerializeField] public float MoveDistance { get; private set; } = 5f;
    [field: SerializeField] public float MoveDuration { get; private set; } = 0.1f;
    [field: SerializeField] public float FadeDuration { get; private set; } = 0.5f;
    [field: SerializeField] public Ease MoveEase { get; private set; } = Ease.OutBounce;

    [Header("Color Settings")]
    [field: SerializeField] public Color NormalColor { get; private set; } = Color.white;
    [field: SerializeField] public Color CriticalColor { get; private set; } = Color.yellow;
}