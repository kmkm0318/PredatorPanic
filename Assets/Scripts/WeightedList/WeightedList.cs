using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가중치 리스트 클래스
/// 가중치에 따라 아이템을 랜덤하게 반환할 수 있습니다
/// </summary>
[Serializable]
public class WeightedList<T>
{
    [Header("Weighted Items")]
    [SerializeField] private List<WeightedItem<T>> _items;
    public List<WeightedItem<T>> Items => _items;

    #region 종합 가중치
    private float _totalWeight = 0f;
    public float TotalWeight
    {
        get
        {
            //처음 접근 시 계산
            if (_totalWeight == 0f)
            {
                _totalWeight = 0f;
                foreach (var item in _items)
                {
                    _totalWeight += item.Weight;
                }
            }
            return _totalWeight;
        }
    }
    #endregion

    public WeightedList()
    {
        _items = new();
    }

    #region 아이템 추가, 초기화
    /// <summary>
    /// 아이템 추가 함수
    /// </summary>
    public void AddItem(WeightedItem<T> item)
    {
        //아이템 추가
        _items.Add(item);

        //총 가중치 초기화
        _totalWeight = 0f;
    }

    /// <summary>
    /// 아이템 초기화 함수
    /// </summary>
    public void Clear()
    {
        _items.Clear();
        _totalWeight = 0f;
    }
    #endregion

    #region 아이템 반환
    /// <summary>
    /// 가중치에 따라 아이템을 랜덤하게 반환합니다
    /// </summary>
    public T GetRandomItem()
    {
        //아이템이 없으면 기본값 반환
        if (_items == null || _items.Count == 0) return default;

        //총 가중치가 0이하면 기본값 반환
        if (TotalWeight <= 0f) return default;

        //랜덤 값 생성 및 아이템 선택
        float randomValue = UnityEngine.Random.Range(0f, TotalWeight);
        float cumulativeWeight = 0f;

        foreach (var item in _items)
        {
            cumulativeWeight += item.Weight;
            if (randomValue <= cumulativeWeight)
            {
                return item.Item;
            }
        }

        //오류 시 기본값 반환
        return default;
    }

    /// <summary>
    /// 가중치에 따라 여러 아이템을 랜덤하게 반환합니다
    /// </summary>
    public List<T> GetRandomElements(int count)
    {
        //빈 리스트 생성
        List<T> selectedItems = new();

        //아이템이 없거나 카운트가 0 이하일 경우 빈 리스트 반환
        if (_items == null || _items.Count == 0 || count <= 0)
        {
            return selectedItems;
        }

        //아이템 선택
        for (int i = 0; i < count; i++)
        {
            T selectedItem = GetRandomItem();
            selectedItems.Add(selectedItem);
        }

        //선택된 아이템 리스트 반환
        return selectedItems;
    }
    #endregion
}