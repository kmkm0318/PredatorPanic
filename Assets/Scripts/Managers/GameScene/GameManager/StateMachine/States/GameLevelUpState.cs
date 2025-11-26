using UnityEngine;

/// <summary>
/// 플레이어 레벨업 시 보상을 보여주는 상태
/// </summary>
public class GameLevelUpState : GameBaseState
{
    public GameLevelUpState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //시간 흐름 정지
        Time.timeScale = 0f;

        //현재는 UI Input에 아무 기능 없음
        //추후 ESC로 건너뛰기 및 R 키로 리롤 등을 가능하게 할 수도 있음
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //이벤트 구독
        RegisterEvents();

        //레벨업 보상 UI 표시
        var isShowReward = GameManager.GameUIManager.LevelUpRewardPresenter.TryShowRewards();

        if (!isShowReward)
        {
            //보상을 표시할 수 없으면 바로 이전 상태로 전환
            ChangeState(Factory.LevelUpPreviousState);
        }
    }

    public override void Update() { }

    public override void Exit()
    {
        //시간 흐름 정상화
        Time.timeScale = 1f;

        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);

        //이벤트 구독 해제
        UnregisterEvents();

        //보상 UI 숨기기
        GameManager.GameUIManager.LevelUpRewardPresenter.HideRewards();
    }

    #region 이벤트
    private void RegisterEvents()
    {
        GameManager.GameUIManager.LevelUpRewardPresenter.OnRewardSelected += OnRewardSelected;
    }

    private void UnregisterEvents()
    {
        GameManager.GameUIManager.LevelUpRewardPresenter.OnRewardSelected -= OnRewardSelected;
    }

    private void OnRewardSelected(LevelUpRewardData data)
    {
        GameManager.Player.ApplyLevelUpRewards(data);

        //이전 상태로 복귀
        ChangeState(Factory.LevelUpPreviousState);
    }
    #endregion
}
