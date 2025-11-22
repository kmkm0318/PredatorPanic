/// <summary>
/// 플레이어 상태의 기본 클래스
/// SupwerState와 SubState를 통해 계층적 상태 기계를 구현
/// </summary>
public abstract class PlayerBaseState : IState
{
    protected PlayerController Owner { get; private set; }
    protected PlayerBaseState SuperState { get; private set; }
    protected PlayerBaseState SubState { get; private set; }
    public PlayerBaseState(PlayerController owner) { Owner = owner; }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    public abstract void InitSubState();
    public abstract void CheckChangeState();

    /// <summary>
    /// 상태 전환 함수
    /// SuperState가 없으면 Owner의 상태 기계를 직접 변경
    /// SuperState가 있으면 SuperState의 SubState로 새 상태를 설정
    /// </summary>
    protected void ChangeState(PlayerBaseState newState)
    {
        if (SuperState == null)
        {
            Owner.StateMachine.ChangeState(newState);
        }
        else
        {
            SuperState.ChangeSubState(newState);
        }
    }

    protected void ChangeSubState(PlayerBaseState newSubState)
    {
        SubState?.Exit();
        SetSubState(newSubState);
        SubState?.Enter();
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        SuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        SubState = newSubState;
        SubState.SetSuperState(this);
    }
}