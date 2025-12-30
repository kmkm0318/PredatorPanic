using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 파티클 이펙트 매니저 클래스
/// </summary>
public class ParticleEffectManager : Singleton<ParticleEffectManager>
{
    #region 오브젝트 풀
    private Dictionary<ParticleEffectData, ObjectPool<ParticleEffect>> _pools = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(ParticleEffectData data)
    {
        //데이터에 맞는 풀 생성
        ObjectPool<ParticleEffect> pool = new(
            () =>
            {
                var effect = Instantiate(data.ParticleEffectPrefab, transform);
                effect.Init(data, this);
                return effect;
            },
            (effect) => effect.gameObject.SetActive(true),
            (effect) => effect.gameObject.SetActive(false),
            (effect) => Destroy(effect.gameObject)
        );

        //딕셔너리에 추가
        _pools[data] = pool;
    }

    private ObjectPool<ParticleEffect> GetPool(ParticleEffectData data)
    {
        //풀이 없을 시
        if (!_pools.TryGetValue(data, out var pool))
        {
            //초기화
            InitPool(data);

            //풀 가져오기
            pool = _pools[data];
        }

        return pool;
    }

    public void Release(ParticleEffect effect)
    {
        //데이터 가져오기
        ParticleEffectData data = effect.Data;

        //풀이 있을 시
        if (data != null && _pools.TryGetValue(data, out var pool))
        {
            //풀에 반환
            pool.Release(effect);
        }
        else
        {
            //오브젝트 파괴
            Destroy(effect.gameObject);
        }
    }
    #endregion

    #region 플레이
    public void Play(ParticleEffectData data, Vector3 position, Quaternion rotation, Action onComplete = null)
    {
        //데이터 없을 시 패스
        if (data == null) return;

        //풀 가져오기
        var pool = GetPool(data);

        //풀에서 이펙트 가져오기
        var effect = pool.Get();

        //이펙트 위치, 방향 설정
        effect.transform.SetPositionAndRotation(position, rotation);

        //이펙트 재생
        effect.Play(onComplete);
    }
    #endregion
}