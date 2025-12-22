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
    #region 데이터
    public AudioData AudioData { get; private set; }
    #endregion

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

    #region 초기화
    public void Init(AudioMixerGroup sfxMixerGroup)
    {
        // 오디오 믹서 그룹 할당
        _audioSource.outputAudioMixerGroup = sfxMixerGroup;
    }
    #endregion

    #region 효과음 재생 및 콜백
    public void PlaySfx(AudioData audioData, Vector3 position, Action<SfxObject> onComplete = null)
    {
        // 데이터 저장
        AudioData = audioData;

        // 위치 설정
        transform.position = position;

        // 볼륨 설정
        _audioSource.volume = audioData.Volume;

        // 피치 랜덤 설정
        float finalPitch = audioData.Pitch + UnityEngine.Random.Range(-audioData.PitchRandomness, audioData.PitchRandomness);
        _audioSource.pitch = finalPitch;

        // 클립 설정 및 재생
        _audioSource.clip = audioData.AudioClip;
        _audioSource.Play();

        // 2D/3D 설정
        _audioSource.spatialBlend = audioData.Is2D ? 0f : 1f;

        // 완료 콜백 등록
        OnComplete = onComplete;

        // 재생 완료 코루틴 시작
        StartCoroutine(SfxPlayCoroutine(audioData.AudioClip.length / finalPitch));
    }

    private IEnumerator SfxPlayCoroutine(float duration)
    {
        // 재생 시간 대기
        yield return new WaitForSeconds(duration);

        // 재생 완료 콜백 호출
        OnComplete?.Invoke(this);
    }
    #endregion
}