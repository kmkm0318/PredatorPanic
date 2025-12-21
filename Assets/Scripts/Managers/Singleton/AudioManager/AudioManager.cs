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
    public void PlayBgm(AudioClip clip, float volume = 1f)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.volume = volume;
        _bgmAudioSource.Play();
    }

    public void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float pitchRandomness = 0.1f)
    {
        var sfxObject = _sfxObjectPool.Get();
        sfxObject.PlaySfx(clip, position, volume, pitch, pitchRandomness, OnSfxComplete);
    }

    private void OnSfxComplete(SfxObject sfxObject)
    {
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