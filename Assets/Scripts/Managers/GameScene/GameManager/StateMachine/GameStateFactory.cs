/// <summary>
/// 게임 상태들을 생성하는 팩토리 클래스
/// 게임 상태를 캐싱해둔 후 프로퍼티로 제공
/// </summary>
public class GameStateFactory
{
    private GameManager _gameManager;

    #region 상태 클래스 프로퍼티
    public GameLoadingState Loading { get; private set; }
    public GameRoundStartState RoundStart { get; private set; }
    public GamePlayingState Playing { get; private set; }
    public GamePauseState Pause { get; private set; }
    public GameLevelUpState LevelUp { get; private set; }
    public GameRoundClearState RoundClear { get; private set; }
    public GameShopState Shop { get; private set; }
    public GameClearState Clear { get; private set; }
    public GameOverState Over { get; private set; }
    #endregion

    //레벨 업의 이전 상태를 저장하기 위한 프로퍼티
    public GameBaseState LevelUpPreviousState { get; set; }

    public GameStateFactory(GameManager gameManager)
    {
        _gameManager = gameManager;

        Loading = new GameLoadingState(_gameManager, this);
        RoundStart = new GameRoundStartState(_gameManager, this);
        Playing = new GamePlayingState(_gameManager, this);
        Pause = new GamePauseState(_gameManager, this);
        LevelUp = new GameLevelUpState(_gameManager, this);
        RoundClear = new GameRoundClearState(_gameManager, this);
        Shop = new GameShopState(_gameManager, this);
        Clear = new GameClearState(_gameManager, this);
        Over = new GameOverState(_gameManager, this);
    }
}