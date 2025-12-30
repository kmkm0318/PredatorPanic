using System;
using UnityEngine;

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

        //이벤트 해제
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        var pausePresenter = GameManager.GameUIManager.PausePresenter;
        pausePresenter.OnResumeRequested += HandleOnResumeRequested;
        pausePresenter.OnMainMenuRequested += HandleOnMainMenuRequested;
    }

    private void UnregisterEvents()
    {
        var pausePresenter = GameManager.GameUIManager.PausePresenter;
        pausePresenter.OnResumeRequested -= HandleOnResumeRequested;
        pausePresenter.OnMainMenuRequested -= HandleOnMainMenuRequested;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResumeRequested()
    {
        //게임 플레이 상태로 전환
        ChangeState(Factory.Playing);
    }

    private void HandleOnMainMenuRequested()
    {
        //게임 플레이 상태로 전환
        ChangeState(Factory.Playing);

        //플레이어 체력 0으로 설정
        GameManager.Player.DamageFull();
    }
    #endregion
}
