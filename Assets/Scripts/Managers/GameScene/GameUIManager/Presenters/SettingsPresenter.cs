using System;
using System.Collections.Generic;

/// <summary>
/// 설정 UI 프레젠터
/// </summary>
public class SettingsPresenter : IPresenter
{
    #region 레퍼런스
    private SettingsManager _settingsManager;
    private SettingsUI _settingsUI;
    #endregion

    #region 이벤트
    public event Action OnClosed;
    #endregion

    //생성자
    public SettingsPresenter(SettingsManager settingsManager, SettingsUI settingsUI)
    {
        _settingsManager = settingsManager;
        _settingsUI = settingsUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        //이벤트 등록
        RegisterEvents();

        //UI 초기화
        _settingsUI.Init();

        //UI 초기 설정
        InitUI();
    }

    private void InitUI()
    {
        //데이터 가져오기
        var data = _settingsManager.CurrentData;

        //해상도 옵션 문자열 리스트 생성
        List<string> resolutionStrings = new();

        foreach (var res in _settingsManager.ResolutionOptions)
        {
            resolutionStrings.Add($"{res.Item1} x {res.Item2}");
        }

        //UI에 데이터 반영
        _settingsUI.SetResolutionDropdown(resolutionStrings, data.ResolutionIndex);
        _settingsUI.SetRefreshRateSlider(data.RefreshRate);
        _settingsUI.SetFullScreenToggle(data.IsFullScreen);
        _settingsUI.SetMasterVolumeSlider(data.MasterVolume);
        _settingsUI.SetBGMVolumeSlider(data.BGMVolume);
        _settingsUI.SetSFXVolumeSlider(data.SFXVolume);
    }

    public void Reset()
    {
        //이벤트 해제
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        _settingsUI.OnResolutionIndexChanged += HandleOnResolutionIndexChanged;
        _settingsUI.OnFullScreenToggled += HandleOnFullScreenToggled;
        _settingsUI.OnRefreshRateChanged += HandleOnRefreshRateChanged;
        _settingsUI.OnMasterVolumeChanged += HandleOnMasterVolumeChanged;
        _settingsUI.OnBGMVolumeChanged += HandleOnBGMVolumeChanged;
        _settingsUI.OnSFXVolumeChanged += HandleOnSFXVolumeChanged;
        _settingsUI.OnSaveButtonClicked += HandleOnSaveButtonClicked;
        _settingsUI.OnCloseButtonClicked += HandleOnCloseButtonClicked;
    }

    private void UnregisterEvents()
    {
        _settingsUI.OnResolutionIndexChanged -= HandleOnResolutionIndexChanged;
        _settingsUI.OnFullScreenToggled -= HandleOnFullScreenToggled;
        _settingsUI.OnRefreshRateChanged -= HandleOnRefreshRateChanged;
        _settingsUI.OnMasterVolumeChanged -= HandleOnMasterVolumeChanged;
        _settingsUI.OnBGMVolumeChanged -= HandleOnBGMVolumeChanged;
        _settingsUI.OnSFXVolumeChanged -= HandleOnSFXVolumeChanged;
        _settingsUI.OnSaveButtonClicked -= HandleOnSaveButtonClicked;
        _settingsUI.OnCloseButtonClicked -= HandleOnCloseButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResolutionIndexChanged(int resolutionIndex)
    {
        _settingsManager.CurrentData.ResolutionIndex = resolutionIndex;
    }

    private void HandleOnFullScreenToggled(bool isFullScreen)
    {
        _settingsManager.CurrentData.IsFullScreen = isFullScreen;
    }

    private void HandleOnRefreshRateChanged(int refreshRate)
    {
        _settingsManager.CurrentData.RefreshRate = refreshRate;
    }

    private void HandleOnMasterVolumeChanged(float masterVolume)
    {
        _settingsManager.CurrentData.MasterVolume = masterVolume;
    }

    private void HandleOnBGMVolumeChanged(float bgmVolume)
    {
        _settingsManager.CurrentData.BGMVolume = bgmVolume;
    }

    private void HandleOnSFXVolumeChanged(float sfxVolume)
    {
        _settingsManager.CurrentData.SFXVolume = sfxVolume;
    }

    private void HandleOnSaveButtonClicked()
    {
        _settingsManager.ApplySettings();
        _settingsManager.SaveSettings();
    }

    private void HandleOnCloseButtonClicked()
    {
        _settingsUI.Hide(0f);
        OnClosed?.Invoke();
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        InitUI();
        _settingsUI.Show(0f);
    }

    public void Hide() => _settingsUI.Hide(0f);
    #endregion
}