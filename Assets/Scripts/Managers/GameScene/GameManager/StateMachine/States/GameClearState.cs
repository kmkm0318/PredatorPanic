using UnityEngine;

/// <summary>
/// 게임 클리어 시의 상태 클래스
/// 목표 라운드를 클리어했을 시의 상태
/// </summary>
public class GameClearState : GameBaseState
{
    public GameClearState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //유저 세이브 데이터 가져오기
        var userSaveData = UserSaveDataManager.Instance.UserSaveData;

        //플레이어의 DNA 재화를 UserSaveData에 반영
        userSaveData.DNA += GameManager.Player.DNA;

        //최대, 최소값 클램핑
        userSaveData.DNA = Mathf.Clamp(userSaveData.DNA, 0, int.MaxValue);

        //저장
        UserSaveDataManager.Instance.SaveUserSaveData();

        //TODO: 게임 클리어 UI 표시 및 기타 처리
        $"Game Cleared!".Log();
    }

    public override void Update() { }

    public override void Exit() { }
}
