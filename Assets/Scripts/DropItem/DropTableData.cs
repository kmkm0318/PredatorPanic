using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드롭 테이블 데이터
/// 아이템 드롭 정보를 담고 있는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "DropTableData", menuName = "SO/DropItem/DropTableData", order = 0)]
public class DropTableData : ScriptableObject
{
    [SerializeField] private List<DropItemData> _dropItems;
    public List<DropItemData> DropItems => _dropItems;
}