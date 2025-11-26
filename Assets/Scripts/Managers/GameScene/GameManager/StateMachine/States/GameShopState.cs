using System;

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

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //이벤트 구독
        RegisterEvents();
    }

    public override void Update() { }

    public override void Exit()
    {
        //상점 UI 비활성화
        GameManager.GameUIManager.ShopPresenter.HideShopUI();

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);

        //이벤트 구독 해제
        UnregisterEvents();
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
