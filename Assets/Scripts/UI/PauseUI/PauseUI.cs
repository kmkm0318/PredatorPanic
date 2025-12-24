using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 일시정지 UI 클래스
/// </summary>
public class PauseUI : ShowHideUI
{
    [Header("UI Elements")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;

    #region 이벤트
    public event Action OnResumeButtonClicked;
    public event Action OnSettingsButtonClicked;
    public event Action OnMainMenuButtonClicked;
    #endregion

    public void Init()
    {
        //버튼 이벤트 설정
        _resumeButton.onClick.AddListener(() => OnResumeButtonClicked?.Invoke());
        _settingsButton.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
        _mainMenuButton.onClick.AddListener(() => OnMainMenuButtonClicked?.Invoke());
    }
}