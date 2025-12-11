using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 테이블 리스트 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyTableDataList", menuName = "SO/EnemyTable/EnemyTableDataList", order = 0)]
public class EnemyTableDataList : ScriptableObject
{
    [Header("Enemy Table Data List")]
    [field: SerializeField] public List<EnemyTableData> EnemyTableDatas { get; private set; }
}