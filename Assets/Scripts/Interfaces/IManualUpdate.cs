/// <summary>
/// 수동 업데이트 인터페이스
/// </summary>
public interface IManualUpdate
{
    bool IsActive { get; set; }
    void ManualUpdate(float deltaTime);
}