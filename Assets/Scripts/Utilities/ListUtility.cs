using System.Collections.Generic;

public static class ListUtility
{
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