using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 UI 클래스
/// </summary>
public class MainMenuUI : ShowHideUI
{
    [Header("UI Elements")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _evolutionButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    public event Action OnStartButtonClicked;
    public event Action OnEvolutionButtonClicked;
    public event Action OnSettingsButtonClicked;
    public event Action OnExitButtonClicked;

    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        _startButton.onClick.AddListener(() => OnStartButtonClicked?.Invoke());
        _evolutionButton.onClick.AddListener(() => OnEvolutionButtonClicked?.Invoke());
        _settingsButton.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
        _exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
    }
}