using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 메인 메뉴의 UI 관리를 담당하는 클래스
/// </summary>
public class MainMenuUIManager : MonoBehaviour, ICancelableManager
{
    #region 씬 내 UI
    [SerializeField] private TooltipUI _tooltipUI;
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private ConfirmPopupUI _confirmPopupUI;
    #endregion

    #region MVP 구조를 위한 Presenter들
    public TooltipPresenter TooltipPresenter { get; private set; }
    public SettingsPresenter SettingsPresenter { get; private set; }
    public ConfirmPopupPresenter ConfirmPopupPresenter { get; private set; }
    #endregion

    #region 변수
    private Stack<ICancelable> _cancelableStack = new();
    #endregion

    #region 레퍼런스
    private MainMenuManager _mainMenuManager;
    #endregion

    public void Init(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;

        RegisterEvents();

        InitPresenter();
    }

    private void InitPresenter()
    {
        TooltipPresenter = new TooltipPresenter(_tooltipUI);
        TooltipPresenter.Init();

        ConfirmPopupPresenter = new ConfirmPopupPresenter(_confirmPopupUI, this);
        ConfirmPopupPresenter.Init();

        SettingsPresenter = new SettingsPresenter(SettingsManager.Instance, _settingsUI, ConfirmPopupPresenter, this);
        SettingsPresenter.Init();
    }

    private void OnDestroy()
    {
        TooltipPresenter.Reset();
        ConfirmPopupPresenter.Reset();
        SettingsPresenter.Reset();

        UnregisterEvents();
    }

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        //인풋 액션 가져오기
        var inputActions = InputManager.Instance.PlayerInputActions;

        //Cancel 이벤트 구독
        inputActions.UI.Cancel.performed += HandleOnCancelPerformed;
    }

    private void UnregisterEvents()
    {
        //인풋 액션 가져오기
        var inputActions = InputManager.Instance.PlayerInputActions;

        //Cancel 이벤트 해제
        inputActions.UI.Cancel.performed -= HandleOnCancelPerformed;
    }

    private void HandleOnCancelPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_cancelableStack.Count > 0)
        {
            //가장 위에 있는 ICancelable 객체의 Cancel 메서드 호출
            var topCancelable = _cancelableStack.Peek();
            topCancelable.Cancel();
        }
    }
    #endregion

    #region ICancelable 추가 및 제거
    public void PushCancelable(ICancelable cancelable)
    {
        _cancelableStack.Push(cancelable);
    }

    public void PopCancelable(ICancelable cancelable)
    {
        if (_cancelableStack.Count > 0 && _cancelableStack.Peek() == cancelable)
        {
            _cancelableStack.Pop();
        }
    }
    #endregion
}