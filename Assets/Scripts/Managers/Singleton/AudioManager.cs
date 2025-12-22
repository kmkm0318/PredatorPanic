using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

/// <summary>
/// 오디오 매니저 싱글톤 클래스
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    #region 상수
    private const string MASTER_VOLUME = "MasterVolume";
    private const string BGM_VOLUME = "BGMVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const int MAX_CONCURRENT_SFX = 10;
    #endregion

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup _masterMixerGroup;
    [SerializeField] private AudioMixerGroup _bgmMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmAudioSource;

    [Header("SFX Audio Source Prefab")]
    [SerializeField] private SfxObject _sfxObjectPrefab;

    #region 오브젝트 풀
    private ObjectPool<SfxObject> _sfxObjectPool;
    private Dictionary<AudioData, int> _activeSfxCount = new();
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    #region 초기화
    private void Init()
    {
        // BGM 오디오 소스 초기화
        _bgmAudioSource.outputAudioMixerGroup = _bgmMixerGroup;

        // 효과 오디오 소스 풀 초기화
        InitPool();
    }
    #endregion

    #region 오브젝트 풀링
    private void InitPool()
    {
        _sfxObjectPool = new(
            () =>
            {
                var sfxObject = Instantiate(_sfxObjectPrefab, transform);
                // 오디오 믹서 그룹 할당
                sfxObject.Init(_sfxMixerGroup);
                return sfxObject;
            },
            (sfxObject) => sfxObject.gameObject.SetActive(true),
            (sfxObject) => sfxObject.gameObject.SetActive(false),
            (sfxObject) => Destroy(sfxObject.gameObject)
        );
    }
    #endregion

    #region 오디오 재생
    /// <summary>
    /// AudioData를 통한 배경음 재생
    /// </summary>
    public void PlayBgm(AudioData audioData)
    {
        // null 체크
        if (audioData == null || audioData.AudioClip == null) return;

        // 클립 설정
        _bgmAudioSource.clip = audioData.AudioClip;

        // 볼륨 설정
        _bgmAudioSource.volume = audioData.Volume;

        // 피치 설정
        _bgmAudioSource.pitch = audioData.Pitch;

        // 재생
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// AudioData를 통한 효과음 재생
    /// </summary>
    public void PlaySfx(AudioData audioData, Vector3 position)
    {
        // null 체크
        if (audioData == null || audioData.AudioClip == null) return;

        // 동시 재생 제한 체크
        if (!CanPlaySfx(audioData)) return;

        // 동시 재생 카운트 증가
        _activeSfxCount[audioData]++;

        // sfxObject 가져오기
        var sfxObject = _sfxObjectPool.Get();

        // 효과음 재생
        sfxObject.PlaySfx(audioData, position, OnSfxComplete);
    }

    private bool CanPlaySfx(AudioData audioData)
    {
        if (_activeSfxCount.TryGetValue(audioData, out var count))
        {
            return count < MAX_CONCURRENT_SFX;
        }
        else
        {
            _activeSfxCount[audioData] = 0;
            return true;
        }
    }

    private void OnSfxComplete(SfxObject sfxObject)
    {
        // AudioData 체크
        if (sfxObject.AudioData != null && _activeSfxCount.ContainsKey(sfxObject.AudioData))
        {
            // 동시 재생 카운트 감소
            _activeSfxCount[sfxObject.AudioData]--;
        }

        // sfxObject 반환
        _sfxObjectPool.Release(sfxObject);
    }
    #endregion

    #region 볼륨 변경
    public void SetMasterVolume(float volume)
    {
        var decibel = VolumeToDecibel(volume);
        _masterMixerGroup.audioMixer.SetFloat(MASTER_VOLUME, decibel);
    }

    public void SetBgmVolume(float volume)
    {
        var decibel = VolumeToDecibel(volume);
        _bgmMixerGroup.audioMixer.SetFloat(BGM_VOLUME, decibel);
    }

    public void SetSfxVolume(float volume)
    {
        var decibel = VolumeToDecibel(volume);
        _sfxMixerGroup.audioMixer.SetFloat(SFX_VOLUME, decibel);
    }

    //오디오 믹서의 볼륨은 데시벨 단위이므로 변환 필요
    private float VolumeToDecibel(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }
    #endregion
}