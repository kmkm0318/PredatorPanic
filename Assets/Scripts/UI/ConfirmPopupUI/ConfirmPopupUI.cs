using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 확인 팝업 UI 클래스
/// </summary>
public class ConfirmPopupUI : ShowHideUI
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    #region 이벤트
    public event Action OnConfirmed;
    public event Action OnCancelled;
    #endregion

    private void Awake()
    {
        Init();
    }

    #region 초기화
    private void Init()
    {
        _confirmButton.onClick.AddListener(HandleConfirmClicked);
        _cancelButton.onClick.AddListener(HandleCancelClicked);
    }

    private void HandleConfirmClicked()
    {
        OnConfirmed?.Invoke();
    }

    private void HandleCancelClicked()
    {
        OnCancelled?.Invoke();
    }
    #endregion

    public void SetMessage(string message)
    {
        _messageText.text = message;
    }
}