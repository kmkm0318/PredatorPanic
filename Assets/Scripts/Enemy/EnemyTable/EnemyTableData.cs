using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 스폰을 위한 데이터 클래스
/// 보스와 일반 적 포함
/// 일반 적은 가중치 리스트를 통해서 가중치에 따라 랜덤하게 스폰 가능
/// </summary>
[CreateAssetMenu(fileName = "EnemyTableData", menuName = "SO/EnemyTable/EnemyTableData", order = 0)]
public class EnemyTableData : ScriptableObject
{
    [Header("Weighted Enemy Data List")]
    [field: SerializeField] public List<EnemyData> BossEnemyDatas { get; private set; }
    [field: SerializeField] public WeightedList<EnemyData> EnemyDatas { get; private set; }
}