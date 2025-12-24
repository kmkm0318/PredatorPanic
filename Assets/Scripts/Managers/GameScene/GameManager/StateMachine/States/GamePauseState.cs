using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 일시정지 상태
/// </summary>
public class GamePauseState : GameBaseState
{
    public GamePauseState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //플레이어 스탯 UI 표시
        GameManager.GameUIManager.PlayerStatPresenter.Show();

        //시간 흐름 정지
        Time.timeScale = 0f;

        //UI 인풋 모드로 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //정지 UI 표시
        GameManager.GameUIManager.PausePresenter.Show();

        //이벤트 구독
        RegisterEvents();
    }

    public override void Update() { }

    public override void Exit()
    {
        //플레이어 스탯 UI 숨기기
        GameManager.GameUIManager.PlayerStatPresenter.Hide();

        //시간 흐름 정상화
        Time.timeScale = 1f;

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);

        //정지 UI 숨기기
        GameManager.GameUIManager.PausePresenter.Hide();

        //이벤트 해제
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;
        inputActions.UI.Resume.performed += HandleOnResumePerformed;

        GameManager.GameUIManager.PausePresenter.OnResumeButtonClicked += HandleOnResumeButtonClicked;
    }

    private void UnregisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;
        inputActions.UI.Resume.performed -= HandleOnResumePerformed;

        GameManager.GameUIManager.PausePresenter.OnResumeButtonClicked -= HandleOnResumeButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResumePerformed(InputAction.CallbackContext context)
    {
        ChangeState(Factory.Playing);
    }

    private void HandleOnResumeButtonClicked()
    {
        ChangeState(Factory.Playing);
    }
    #endregion
}
