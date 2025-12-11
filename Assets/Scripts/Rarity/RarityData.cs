using UnityEngine;

/// <summary>
/// 희귀도 데이터 클래스
/// 희귀도의 이름, 색상 등을 저장합니다
/// </summary>
[CreateAssetMenu(fileName = "RarityData", menuName = "SO/Rarity/RarityData", order = 0)]
public class RarityData : ScriptableObject
{
    [Header("Rarity Info")]
    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public Color RarityColor { get; private set; }
    [field: SerializeField] public string RarityName { get; private set; }
}