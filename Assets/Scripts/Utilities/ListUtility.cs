using System.Collections.Generic;

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

        int idx = UnityEngine.Random.Range(0, list.Count);
        return list[idx];
    }
}