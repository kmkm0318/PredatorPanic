using System;
using UnityEngine;

/// <summary>
/// 가중치 리스트를 위한 가중치 아이템 클래스
/// </summary>
[Serializable]
public class WeightedItem<T>
{
    [field: SerializeField] public T Item { get; private set; }
    //가중치는 0 이상
    [field: SerializeField, Min(0f)] public float Weight { get; private set; }

    public WeightedItem(T item, float weight)
    {
        Item = item;
        //가중치는 0 이상
        Weight = Mathf.Max(0f, weight);
    }
}