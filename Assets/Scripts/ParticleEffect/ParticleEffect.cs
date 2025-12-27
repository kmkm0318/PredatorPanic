using System;
using UnityEngine;

/// <summary>
/// 파티클 이펙트 클래스
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
    #region 데이터
    public ParticleEffectData Data { get; private set; }
    #endregion

    #region 레퍼런스
    private ParticleEffectManager _particleEffectManager;
    #endregion

    #region 컴포넌트
    private ParticleSystem _particleSystem;
    #endregion

    #region 이벤트
    private event Action _onComplete;
    #endregion

    public void Init(ParticleEffectData data, ParticleEffectManager particleEffectManager)
    {
        Data = data;
        _particleEffectManager = particleEffectManager;

        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play(Action onComplete = null)
    {
        _onComplete = onComplete;
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _onComplete?.Invoke();
        _particleEffectManager.Release(this);
    }
}