using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SFX 오디오 재생을 위한 오브젝트 클래스
/// 효과음의 pitch를 랜덤하게 변경하여 재생
/// 재생 완료 후 오디오 매니저의 풀로 반환
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SfxObject : MonoBehaviour
{
    #region 레퍼런스
    private AudioSource _audioSource;
    #endregion

    #region 이벤트
    private event Action<SfxObject> OnComplete;
    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(AudioMixerGroup sfxMixerGroup)
    {
        // 오디오 믹서 그룹 할당
        _audioSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    public void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float pitchRandomness = 0.1f, Action<SfxObject> onComplete = null)
    {
        // 위치 설정
        transform.position = position;

        // 볼륨 설정
        _audioSource.volume = volume;

        // 피치 랜덤 설정
        float finalPitch = pitch + UnityEngine.Random.Range(-pitchRandomness, pitchRandomness);
        _audioSource.pitch = finalPitch;

        // 클립 설정 및 재생
        _audioSource.clip = clip;
        _audioSource.Play();

        // 완료 콜백 등록
        OnComplete = onComplete;

        // 재생 완료 코루틴 시작
        StartCoroutine(SfxPlayCoroutine(clip.length / finalPitch));
    }

    private IEnumerator SfxPlayCoroutine(float duration)
    {
        // 재생 시간 대기
        yield return new WaitForSeconds(duration);

        // 재생 완료 콜백 호출
        OnComplete?.Invoke(this);
    }
}