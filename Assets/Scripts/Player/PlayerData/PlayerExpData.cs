using UnityEngine;

/// <summary>
/// 플레이어의 경험치 관련 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "PlayerExpData", menuName = "SO/Player/PlayerExpData", order = 0)]
public class PlayerExpData : ScriptableObject
{
    [Header("EXP Settings")]
    [SerializeField] private float _baseExp = 100f;
    [SerializeField] private float _expGrowthRate = 1.1f;
    public float BaseExp => _baseExp;
    public float ExpGrowthRate => _expGrowthRate;
}