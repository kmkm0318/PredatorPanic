using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드롭 아이템 테이블 데이터
/// 아이템 드롭 정보를 담고 있는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "DropItemTableData", menuName = "SO/DropItem/DropItemTableData", order = 0)]
public class DropItemTableData : ScriptableObject
{
    [SerializeField] private List<DropItemTableEntry> _dropItemTableEntries;
    public List<DropItemTableEntry> DropItemTableEntries => _dropItemTableEntries;
}
