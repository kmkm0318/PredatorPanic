/// <summary>
/// 게임 오버 시의 상태 클래스
/// </summary>
public class GameOverState : GameBaseState
{
    public GameOverState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //유저 데이터에 DNA 추가
        UserSaveDataManager.Instance.AddDNA(GameManager.Player.DNA);

        //데이터 저장
        UserSaveDataManager.Instance.SaveUserSaveData();

        //인풋 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //게임 오버  UI 표시
        GameManager.GameUIManager.GameResultPresenter.ShowGameResult("게임 오버!", GameManager.Player.DNA);

        //게임 오버 BGM 재생
        AudioManager.Instance.ChangeBGM(GameManager.GameData.GameOverBGMAudioData);
    }

    public override void Update() { }

    public override void Exit() { }
}
