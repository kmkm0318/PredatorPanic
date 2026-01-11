using UnityEngine;

/// <summary>
/// 적 공격 표시 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "IndicatedAttackData", menuName = "SO/Projectile/IndicatedAttack/IndicatedAttackData", order = 0)]
public class IndicatedAttackData : ScriptableObject
{
    [Header("Indicated Attack Prefab")]
    [SerializeField] private IndicatedAttack _indicatedAttackPrefab;
    public IndicatedAttack IndicatedAttackPrefab => _indicatedAttackPrefab;

    [Header("Indicated Attack Data")]
    [SerializeField] private LayerMask _targetLayer;
    public LayerMask TargetLayer => _targetLayer;
}