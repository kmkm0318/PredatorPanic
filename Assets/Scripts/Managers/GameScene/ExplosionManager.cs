using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 폭발 관리자 클래스
/// </summary>
public class ExplosionManager : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<ExplosionData, ObjectPool<Explosion>> _explosions = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(ExplosionData data)
    {
        if (_explosions.ContainsKey(data))
            return;

        ObjectPool<Explosion> pool = new(
            () =>
            {
                var explosion = Instantiate(data.ExplosionPrefab, transform);
                explosion.Init(data, this);
                return explosion;
            },
            (explosion) => explosion.gameObject.SetActive(true),
            (explosion) => explosion.gameObject.SetActive(false),
            (explosion) => Destroy(explosion.gameObject)
        );

        _explosions[data] = pool;
    }

    private ObjectPool<Explosion> GetPool(ExplosionData data)
    {
        if (!_explosions.TryGetValue(data, out var pool))
        {
            InitPool(data);
            pool = _explosions[data];
        }

        return pool;
    }
    #endregion

    /// <summary>
    /// 폭발 스폰
    /// </summary>
    public Explosion GetExplosion(ExplosionData data)
    {
        var pool = GetPool(data);
        return pool.Get();
    }

    /// <summary>
    /// 폭발 반환
    /// </summary>
    public void ReleaseExplosion(Explosion explosion)
    {
        var pool = GetPool(explosion.ExplosionData);
        pool.Release(explosion);
    }
}