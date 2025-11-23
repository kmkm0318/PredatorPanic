/// <summary>
/// 게임 상태를 위한 베이스 상태 클래스
/// </summary>
public abstract class GameBaseState : IState
{
    protected GameManager GameManager { get; private set; }
    protected GameStateFactory Factory { get; private set; }

    public GameBaseState(GameManager gameManager, GameStateFactory factory)
    {
        GameManager = gameManager;
        Factory = factory;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    public virtual void InitSubStates() { }

    protected void ChangeState(GameBaseState newState)
    {
        GameManager.StateMachine.ChangeState(newState);
    }
}