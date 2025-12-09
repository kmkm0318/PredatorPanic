using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 스폰을 위한 가중치 리스트 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyTableData", menuName = "SO/EnemyTable/EnemyTableData", order = 0)]
public class EnemyTableData : ScriptableObject
{
    [Header("Weighted Enemy Data List")]
    [field: SerializeField] public List<EnemyData> BossEnemyDatas { get; private set; }
    [field: SerializeField] public WeightedList<EnemyData> EnemyDatas { get; private set; }
}