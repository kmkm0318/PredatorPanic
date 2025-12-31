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
    private List<Bullet> _activeBullets = new();
    #endregion

    #region 레퍼런스
    public GameManager GameManager { get; private set; }
    #endregion

    public void Init(GameManager gameManager)
    {
        GameManager = gameManager;
    }

    private void Update()
    {
        ManualUpdateActiveBullets();
    }

    private void ManualUpdateActiveBullets()
    {
        //시간 캐싱
        float deltaTime = Time.deltaTime;

        //뒤에서부터 순회하는 것으로 문제 방지
        for (int i = _activeBullets.Count - 1; i >= 0; i--)
        {
            var bullet = _activeBullets[i];

            //Active 상태일 시 수동 업데이트
            if (bullet.IsActive)
            {
                bullet.ManualUpdate(deltaTime);
            }

            //Active 상태가 아니거나 업데이트 후 비활성화된 총알 처리
            if (!bullet.IsActive)
            {
                //맨 뒤 총알로 교체
                int lastIndex = _activeBullets.Count - 1;
                _activeBullets[i] = _activeBullets[lastIndex];

                //목록에서 제거
                _activeBullets.RemoveAt(lastIndex);
            }
        }
    }

    #region 오브젝트 풀링
    private void InitPool(BulletData data)
    {
        if (_bullets.ContainsKey(data))
            return;

        ObjectPool<Bullet> pool = new(
            () =>
            {
                var bullet = Instantiate(data.BulletPrefab, transform);
                bullet.Init(data, this);
                return bullet;
            },
            (bullet) => bullet.gameObject.SetActive(true),
            (bullet) => bullet.gameObject.SetActive(false),
            (bullet) => Destroy(bullet.gameObject)
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
        //풀 가져오기
        var pool = GetPool(data);

        //총알 가져오기
        var bullet = pool.Get();

        //활성 총알 목록에 추가
        _activeBullets.Add(bullet);

        //총알 반환
        return bullet;
    }

    /// <summary>
    /// 총알 반환
    /// </summary>
    public void ReleaseBullet(Bullet bullet)
    {
        //풀 가져오기
        var pool = GetPool(bullet.Data);

        //풀에 반환
        pool.Release(bullet);
    }
}