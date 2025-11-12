/// <summary>
/// 스탯 엔티티 인터페이스
/// 에디터에서 스탯 타입과 값을 쌍으로 다루기 위한 인터페이스
/// </summary>
public interface IStatEntity<T> where T : System.Enum
{
    T StatType { get; }
    float Value { get; }
}