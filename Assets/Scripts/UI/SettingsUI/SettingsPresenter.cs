using System;
using System.Collections.Generic;

/// <summary>
/// 설정 UI 프레젠터
/// </summary>
public class SettingsPresenter : IPresenter, ICancelable
{
    #region 상수
    private const float SLIDER_MIN_DIFFERENCE = 0.01f;
    #endregion

    #region 레퍼런스
    private SettingsManager _settingsManager;
    private SettingsUI _settingsUI;
    private ConfirmPopupPresenter _confirmPopupPresenter;
    private ICancelableManager _cancelableManager;
    #endregion

    #region 변수
    private SettingsData _tempData;
    #endregion

    #region 이벤트
    public event Action OnClosed;
    #endregion

    //생성자
    public SettingsPresenter(SettingsManager settingsManager, SettingsUI settingsUI, ConfirmPopupPresenter confirmPopupPresenter, ICancelableManager cancelableManager)
    {
        _settingsManager = settingsManager;
        _settingsUI = settingsUI;
        _confirmPopupPresenter = confirmPopupPresenter;
        _cancelableManager = cancelableManager;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        //이벤트 등록
        RegisterEvents();

        //UI 초기화
        _settingsUI.Init();
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
        _settingsUI.OnSensitivityChanged += HandleOnSensitivityChanged;
        _settingsUI.OnCameraShakeToggled += HandleOnCameraShakeToggled;
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
        _settingsUI.OnSensitivityChanged -= HandleOnSensitivityChanged;
        _settingsUI.OnCameraShakeToggled -= HandleOnCameraShakeToggled;
        _settingsUI.OnSaveButtonClicked -= HandleOnSaveButtonClicked;
        _settingsUI.OnCloseButtonClicked -= HandleOnCloseButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResolutionIndexChanged(int resolutionIndex)
    {
        _tempData.ResolutionIndex = resolutionIndex;
    }

    private void HandleOnFullScreenToggled(bool isFullScreen)
    {
        _tempData.IsFullScreen = isFullScreen;
    }

    private void HandleOnRefreshRateChanged(int refreshRate)
    {
        _tempData.RefreshRate = refreshRate;
    }

    private void HandleOnMasterVolumeChanged(int masterVolume)
    {
        _tempData.MasterVolume = masterVolume;
    }

    private void HandleOnBGMVolumeChanged(int bgmVolume)
    {
        _tempData.BGMVolume = bgmVolume;
    }

    private void HandleOnSFXVolumeChanged(int sfxVolume)
    {
        _tempData.SFXVolume = sfxVolume;
    }

    private void HandleOnSensitivityChanged(float sensitivity)
    {
        _tempData.Sensitivity = sensitivity;
    }

    private void HandleOnCameraShakeToggled(bool isEnable)
    {
        _tempData.EnableCameraShake = isEnable;
    }

    private void HandleOnSaveButtonClicked()
    {
        //설정 변경
        _settingsManager.ChangeSettings(_tempData);

        //설정 적용
        _settingsManager.ApplySettings();

        //설정 저장
        _settingsManager.SaveSettings();
    }

    private void HandleOnCloseButtonClicked()
    {
        //닫기 호출
        Hide();
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        //UI 초기화
        InitUI();

        //UI 표시
        _settingsUI.Show(0f);

        //Cancelable 매니저에 추가
        _cancelableManager.PushCancelable(this);
    }

    private void InitUI()
    {
        //임시 데이터 생성
        _tempData = new SettingsData(_settingsManager.CurrentData);

        //해상도 옵션 문자열 리스트 생성
        List<string> resolutionStrings = new();

        foreach (var res in _settingsManager.ResolutionOptions)
        {
            resolutionStrings.Add($"{res.Item1} x {res.Item2}");
        }

        //UI에 데이터 반영
        _settingsUI.SetResolutionDropdown(resolutionStrings, _tempData.ResolutionIndex);
        _settingsUI.SetRefreshRateSlider(_tempData.RefreshRate);
        _settingsUI.SetFullScreenToggle(_tempData.IsFullScreen);
        _settingsUI.SetMasterVolumeSlider(_tempData.MasterVolume);
        _settingsUI.SetBGMVolumeSlider(_tempData.BGMVolume);
        _settingsUI.SetSFXVolumeSlider(_tempData.SFXVolume);
        _settingsUI.SetSensitivitySlider(_tempData.Sensitivity);
        _settingsUI.SetCameraShakeToggle(_tempData.EnableCameraShake);
    }

    public void Hide(bool isForce = false)
    {
        //강제로 혹은 변경되지 않았을 시
        if (isForce || !IsChanged())
        {
            //Cancelable 매니저에서 제거
            _cancelableManager.PopCancelable(this);

            //바로 숨기기 및 이벤트 호출
            _settingsUI.Hide(0f, OnClosed);
        }
        else
        {
            //이벤트 구독
            _confirmPopupPresenter.OnConfirmed += HandleOnConfirmed;
            _confirmPopupPresenter.OnCancelled += HandleOnCancelled;

            //메시지 설정
            string message = "설정이 변경되었습니다.\n변경 사항을 저장하지 않고 나가시겠습니까?";

            //팝업 표시
            _confirmPopupPresenter.Show(message);
        }
    }

    private void HandleOnConfirmed()
    {
        //이벤트 해제
        _confirmPopupPresenter.OnConfirmed -= HandleOnConfirmed;
        _confirmPopupPresenter.OnCancelled -= HandleOnCancelled;

        //강제로 닫기
        Hide(true);
    }

    private void HandleOnCancelled()
    {
        //이벤트 해제
        _confirmPopupPresenter.OnConfirmed -= HandleOnConfirmed;
        _confirmPopupPresenter.OnCancelled -= HandleOnCancelled;
    }
    #endregion

    //설정이 변경되었는지 확인
    private bool IsChanged()
    {
        var originalData = _settingsManager.CurrentData;

        return originalData.ResolutionIndex != _tempData.ResolutionIndex ||
               originalData.RefreshRate != _tempData.RefreshRate ||
               originalData.IsFullScreen != _tempData.IsFullScreen ||
               Math.Abs(originalData.MasterVolume - _tempData.MasterVolume) > SLIDER_MIN_DIFFERENCE ||
               Math.Abs(originalData.BGMVolume - _tempData.BGMVolume) > SLIDER_MIN_DIFFERENCE ||
               Math.Abs(originalData.SFXVolume - _tempData.SFXVolume) > SLIDER_MIN_DIFFERENCE ||
               Math.Abs(originalData.Sensitivity - _tempData.Sensitivity) > SLIDER_MIN_DIFFERENCE ||
               originalData.EnableCameraShake != _tempData.EnableCameraShake;
    }

    public void Cancel()
    {
        //닫기 버튼을 누른 것으로 처리
        HandleOnCloseButtonClicked();
    }
}