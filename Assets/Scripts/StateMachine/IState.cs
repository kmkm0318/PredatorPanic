/// <summary>
/// 상태 기계에서 사용하는 상태 인터페이스
/// </summary>
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}