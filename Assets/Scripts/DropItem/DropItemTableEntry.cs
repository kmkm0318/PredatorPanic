using System;
using UnityEngine;

/// <summary>
/// 드롭 아이템 테이블의 한 항목
/// DropRate에 따라 아이템을 드롭
/// MinCount와 MaxCount 사이의 개수를 드롭
/// </summary>
[Serializable]
public class DropItemTableEntry
{
    [Header("Drop Item Info")]
    [SerializeField] private DropItemData _dropItemData;

    [Header("Drop Settings")]
    [SerializeField][Range(0f, 1f)] private float _dropRate;
    [SerializeField] private int _minCount = 1;
    [SerializeField] private int _maxCount = 1;

    public DropItemData DropItemData => _dropItemData;
    public float DropRate => _dropRate;
    public int MinCount => _minCount;
    public int MaxCount => _maxCount;
}