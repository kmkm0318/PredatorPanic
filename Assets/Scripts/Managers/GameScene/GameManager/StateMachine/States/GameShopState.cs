using System;
using UnityEngine;

/// <summary>
/// 게임 상점 상태
/// 상점에서 플레이어가 아이템 등을 구매할 수 있음
/// 플레이어의 무기나 아이템도 판매 가능
/// </summary>
public class GameShopState : GameBaseState
{
    public GameShopState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //상점 UI 활성화
        GameManager.GameUIManager.ShopPresenter.ShowShopUI();

        //플레이어 스탯 UI 표시
        GameManager.GameUIManager.PlayerStatPresenter.Show();

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //시간 흐름 정지
        Time.timeScale = 0f;

        //이벤트 구독
        RegisterEvents();

        //BGM 줄이기
        AudioManager.Instance.SetBGMVolumeRatio(0.5f);
    }

    public override void Update() { }

    public override void Exit()
    {
        //상점 UI 비활성화
        GameManager.GameUIManager.ShopPresenter.HideShopUI();

        //플레이어 스탯 UI 숨기기
        GameManager.GameUIManager.PlayerStatPresenter.Hide();

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);

        //시간 흐름 정상화
        Time.timeScale = 1f;

        //이벤트 구독 해제
        UnregisterEvents();

        //BGM 볼륨 원래대로 복원
        AudioManager.Instance.SetBGMVolumeRatio(1f);
    }

    private void RegisterEvents()
    {
        GameManager.GameUIManager.ShopPresenter.OnNextRoundButtonClicked += OnNextRoundButtonClicked;
    }

    private void UnregisterEvents()
    {
        GameManager.GameUIManager.ShopPresenter.OnNextRoundButtonClicked -= OnNextRoundButtonClicked;
    }

    private void OnNextRoundButtonClicked()
    {
        //라운드 시작 상태로 전환
        ChangeState(Factory.RoundStart);
    }
}
