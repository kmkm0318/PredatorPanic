using UnityEngine;

/// <summary>
/// 게임 오버 시의 상태 클래스
/// </summary>
public class GameOverState : GameBaseState
{
    private const float GAME_OVER_UI_DELAY = 2f;

    public GameOverState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    private float _gameOverDelayTimer = 0f;
    private bool _isGameOverDelayDone = false;

    public override void Enter()
    {
        //유저 데이터에 DNA 추가
        UserSaveDataManager.Instance.AddDNA(GameManager.Player.DNA);

        //데이터 저장
        UserSaveDataManager.Instance.SaveUserSaveData();

        //인풋 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.UI);

        //게임 오버 BGM 재생
        AudioManager.Instance.ChangeBGM(GameManager.GameData.GameOverBGMAudioData);

        //타이머 초기화
        _gameOverDelayTimer = 0f;

        //게임 오버 딜레이 플래그 초기화
        _isGameOverDelayDone = false;
    }

    public override void Update()
    {
        HandleGameOverDelayTimer();
    }

    private void HandleGameOverDelayTimer()
    {
        // 이미 끝난 상태면 진행하지 않음
        if (_isGameOverDelayDone) return;

        //타이머 증가
        _gameOverDelayTimer += Time.deltaTime;

        //딜레이 시간 도달 여부 확인
        if (_gameOverDelayTimer < GAME_OVER_UI_DELAY) return;

        //딜레이 완료 플래그 설정
        _isGameOverDelayDone = true;

        //게임 오버 UI 활성화
        GameManager.GameUIManager.GameResultPresenter.ShowGameResult("게임 오버!", GameManager.Player.DNA);
    }

    public override void Exit() { }
}
