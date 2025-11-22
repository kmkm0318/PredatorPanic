using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 리스트 유틸리티 클래스
/// </summary>
public static class ListUtility
{
    /// <summary>
    /// 리스트에서 랜덤 요소 반환
    /// </summary>
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }

        int idx = Random.Range(0, list.Count);
        return list[idx];
    }

    public static List<T> GetRandomElements<T>(this List<T> list, int count)
    {
        if (list == null || list.Count == 0 || count <= 0)
        {
            return new List<T>();
        }

        List<T> copyList = new(list);
        List<T> selectedElements = new();

        count = Mathf.Min(count, copyList.Count);

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, copyList.Count);
            selectedElements.Add(copyList[idx]);
            copyList.RemoveAt(idx);
        }

        return selectedElements;
    }
}