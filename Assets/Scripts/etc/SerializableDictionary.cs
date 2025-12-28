using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 직렬화 가능한 딕셔너리 클래스
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    #region Key, Value 리스트
    [SerializeField] private List<TKey> _keys = new();
    [SerializeField] private List<TValue> _values = new();
    #endregion

    #region 딕셔너리
    private Dictionary<TKey, TValue> _dictionary;
    public Dictionary<TKey, TValue> Dictionary
    {
        get
        {
            if (_dictionary == null)
            {
                _dictionary = new();
                for (int i = 0; i < Math.Min(_keys.Count, _values.Count); i++)
                {
                    _dictionary[_keys[i]] = _values[i];
                }
            }
            return _dictionary;
        }
    }
    #endregion

    #region 저장하기 전에 실행해야 하는 함수
    public void UpdateList()
    {
        //기존 리스트 초기화
        _keys.Clear();
        _values.Clear();

        //딕셔너리의 키와 값을 리스트에 추가
        foreach (var kvp in Dictionary)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }
    #endregion
}