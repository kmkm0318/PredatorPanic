using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 트레일 관리자 클래스
/// </summary>
public class TrailManager : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<TrailData, ObjectPool<Trail>> _trails = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(TrailData data)
    {
        if (_trails.ContainsKey(data))
            return;

        ObjectPool<Trail> pool = new(
            () =>
            {
                var trail = Instantiate(data.TrailPrefab, transform);
                trail.Init(data);
                return trail;
            },
            (trail) => { trail.gameObject.SetActive(true); },
            (trail) => { trail.gameObject.SetActive(false); },
            (trail) => { Destroy(trail.gameObject); },
            false
        );

        _trails[data] = pool;
    }

    private ObjectPool<Trail> GetPool(TrailData data)
    {
        if (!_trails.TryGetValue(data, out var pool))
        {
            InitPool(data);
            pool = _trails[data];
        }

        return pool;
    }
    #endregion

    /// <summary>
    /// 트레일 스폰
    /// </summary>
    public Trail GetTrail(TrailData data)
    {
        var pool = GetPool(data);
        return pool.Get();
    }

    /// <summary>
    /// 트레일 반환
    /// </summary>
    public void ReleaseTrail(Trail trail)
    {
        trail.transform.SetParent(transform);
        var pool = GetPool(trail.TrailData);
        pool.Release(trail);
    }
}