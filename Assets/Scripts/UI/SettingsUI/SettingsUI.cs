using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 설정 UI 클래스
/// </summary>
public class SettingsUI : ShowHideUI
{
    [Header("Display Settings")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Slider _refreshRateSlider;

    [Header("Audio Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Header("Input Settings")]
    [SerializeField] private Slider _sensitivitySlider;

    [Header("Camera Settings")]
    [SerializeField] private Slider _cameraDistanceSlider;
    [SerializeField] private Toggle _cameraShakeToggle;

    [Header("Buttons")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _closeButton;

    #region 이벤트
    public event Action<int> OnResolutionIndexChanged;
    public event Action<bool> OnFullScreenToggled;
    public event Action<int> OnRefreshRateChanged;
    public event Action<int> OnMasterVolumeChanged;
    public event Action<int> OnBGMVolumeChanged;
    public event Action<int> OnSFXVolumeChanged;
    public event Action<float> OnSensitivityChanged;
    public event Action<float> OnCameraDistanceChanged;
    public event Action<bool> OnCameraShakeToggled;
    public event Action OnSaveButtonClicked;
    public event Action OnCloseButtonClicked;
    #endregion

    #region 초기화
    public void Init()
    {
        //해상도 드롭다운 설정
        _resolutionDropdown.onValueChanged.AddListener(value => OnResolutionIndexChanged?.Invoke(value));

        //전체화면 토글 설정
        _fullScreenToggle.onValueChanged.AddListener(isOn => OnFullScreenToggled?.Invoke(isOn));

        //새로고침 빈도 슬라이더 설정
        _refreshRateSlider.onValueChanged.AddListener(value => OnRefreshRateChanged?.Invoke((int)value));

        //오디오 슬라이더 설정
        _masterVolumeSlider.onValueChanged.AddListener(value => OnMasterVolumeChanged?.Invoke((int)value));
        _bgmVolumeSlider.onValueChanged.AddListener(value => OnBGMVolumeChanged?.Invoke((int)value));
        _sfxVolumeSlider.onValueChanged.AddListener(value => OnSFXVolumeChanged?.Invoke((int)value));

        //민감도 슬라이더 설정
        _sensitivitySlider.onValueChanged.AddListener(value => OnSensitivityChanged?.Invoke(value));

        //카메라 거리 슬라이더 설정
        _cameraDistanceSlider.onValueChanged.AddListener(value => OnCameraDistanceChanged?.Invoke(value));

        //카메라 흔들림 토글 설정
        _cameraShakeToggle.onValueChanged.AddListener(isOn => OnCameraShakeToggled?.Invoke(isOn));

        //저장 버튼 설정
        _saveButton.onClick.AddListener(() => OnSaveButtonClicked?.Invoke());

        //닫기 버튼 설정
        _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
    }
    #endregion

    #region UI 설정 함수
    public void SetResolutionDropdown(List<string> options, int index)
    {
        //기존 옵션 제거
        _resolutionDropdown.ClearOptions();

        //옵션들 추가
        _resolutionDropdown.AddOptions(options);

        //현재 해상도 설정
        _resolutionDropdown.SetValueWithoutNotify(index);
    }

    public void SetFullScreenToggle(bool isFullScreen)
    {
        //전체화면 토글 설정
        _fullScreenToggle.SetIsOnWithoutNotify(isFullScreen);
    }

    public void SetRefreshRateSlider(int refreshRate)
    {
        //새로고침 빈도 슬라이더 설정
        _refreshRateSlider.SetValueWithoutNotify(refreshRate);
    }

    public void SetMasterVolumeSlider(float volume)
    {
        //마스터 볼륨 슬라이더 설정
        _masterVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void SetBGMVolumeSlider(float volume)
    {
        //BGM 볼륨 슬라이더 설정
        _bgmVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void SetSFXVolumeSlider(float volume)
    {
        //SFX 볼륨 슬라이더 설정
        _sfxVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void SetSensitivitySlider(float sensitivity)
    {
        //민감도 슬라이더 설정
        _sensitivitySlider.SetValueWithoutNotify(sensitivity);
    }

    public void SetCameraDistanceSlider(float distance)
    {
        //카메라 거리 슬라이더 설정
        _cameraDistanceSlider.SetValueWithoutNotify(distance);
    }

    public void SetCameraShakeToggle(bool isEnabled)
    {
        //카메라 흔들림 토글 설정
        _cameraShakeToggle.SetIsOnWithoutNotify(isEnabled);
    }
    #endregion
}