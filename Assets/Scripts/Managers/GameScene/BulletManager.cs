using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 총알 관리자 클래스
/// </summary>
public class BulletManager : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<BulletData, ObjectPool<Bullet>> _bullets = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(BulletData data)
    {
        if (_bullets.ContainsKey(data))
            return;

        ObjectPool<Bullet> pool = new(
            () =>
            {
                var bullet = Instantiate(data.BulletPrefab, transform);
                bullet.Init(data);
                return bullet;
            },
            (bullet) => { bullet.gameObject.SetActive(true); },
            (bullet) => { bullet.gameObject.SetActive(false); },
            (bullet) => { Destroy(bullet.gameObject); },
            false
        );

        _bullets[data] = pool;
    }

    private ObjectPool<Bullet> GetPool(BulletData data)
    {
        if (!_bullets.TryGetValue(data, out var pool))
        {
            InitPool(data);
            pool = _bullets[data];
        }

        return pool;
    }
    #endregion

    /// <summary>
    /// 총알 스폰
    /// </summary>
    public Bullet GetBullet(BulletData data)
    {
        var pool = GetPool(data);
        return pool.Get();
    }

    /// <summary>
    /// 총알 반환
    /// </summary>
    public void ReleaseBullet(Bullet bullet)
    {
        var pool = GetPool(bullet.Data);
        pool.Release(bullet);
    }
}