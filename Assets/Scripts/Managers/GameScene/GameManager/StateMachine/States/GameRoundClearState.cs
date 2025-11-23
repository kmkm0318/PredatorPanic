/// <summary>
/// 라운드 클리어 상태
/// </summary>
public class GameRoundClearState : GameBaseState
{
    public GameRoundClearState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //이벤트 구독
        RegisterEvents();

        //모든 적 제거 후 드랍 아이템 수집
        GameManager.EnemyManager.KillAllEnemies();
        GameManager.DropItemManager.CollectAllDropItems(GameManager.Player);
    }

    public override void Update()
    {
        //레벨업 시도 및 성공 시 상태 전환하지 않음
        if (GameManager.Player.TryLevelUp()) return;

        //모든 드랍 아이템이 수집되었는지 확인 후 상점 상태로 전환
        if (!GameManager.DropItemManager.HasActiveDropItems())
        {
            ChangeState(Factory.Shop);
        }
    }

    public override void Exit()
    {
        //이벤트 구독 해제
        UnregisterEvents();
    }

    #region 이벤트
    private void RegisterEvents()
    {
        GameManager.Player.OnLevelUpped += OnLevelUpped;
    }

    private void UnregisterEvents()
    {
        GameManager.Player.OnLevelUpped -= OnLevelUpped;
    }

    private void OnLevelUpped(int obj)
    {
        $"LevelUp: {obj}".Log();
        //레벨업 상태로 전환. 전환 이전에 현재 상태 저장
        Factory.LevelUpPreviousState = this;
        ChangeState(Factory.LevelUp);
    }
    #endregion
}
