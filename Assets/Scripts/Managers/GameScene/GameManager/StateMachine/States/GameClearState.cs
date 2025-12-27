/// <summary>
/// 게임 클리어 시의 상태 클래스
/// 목표 라운드를 클리어했을 시의 상태
/// </summary>
public class GameClearState : GameBaseState
{
    public GameClearState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //유저 데이터에 DNA 추가
        UserSaveDataManager.Instance.AddDNA(GameManager.Player.DNA);

        //데이터 저장
        UserSaveDataManager.Instance.SaveUserSaveData();

        //인풋 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //게임 클리어 UI 표시
        GameManager.GameUIManager.GameResultPresenter.ShowGameResult("게임 클리어!", GameManager.Player.DNA);
    }

    public override void Update() { }

    public override void Exit() { }
}
