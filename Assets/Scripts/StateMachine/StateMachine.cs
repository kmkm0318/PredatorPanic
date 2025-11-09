/// <summary>
/// 상태 기계 클래스
/// </summary>
public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState nextState)
    {
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}