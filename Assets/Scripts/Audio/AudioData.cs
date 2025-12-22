using System;
using UnityEngine;

/// <summary>
/// 오디오 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "AudioData", menuName = "SO/Audio/AudioData", order = 0)]
public class AudioData : ScriptableObject
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip _audioClip;
    public AudioClip AudioClip => _audioClip;

    [Header("Audio Settings")]
    [SerializeField] private float _volume = 1f;
    [SerializeField] private float _pitch = 1f;
    public float Volume => _volume;
    public float Pitch => _pitch;

    [Header("SFX Settings")]
    [SerializeField, Range(0f, 1f)] private float _pitchRandomness = 0.1f;
    [SerializeField] private bool _is2D = false;
    public float PitchRandomness => _pitchRandomness;
    public bool Is2D => _is2D;
}