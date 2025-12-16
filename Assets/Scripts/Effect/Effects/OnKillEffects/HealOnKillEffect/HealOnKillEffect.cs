/// <summary>
/// 킬 시 회복 효과 클래스
/// </summary>
public class HealOnKillEffect : Effect
{
    #region 효과 변수
    private int _currentKillCount;
    private HealOnKillEffectData _data;
    #endregion

    public HealOnKillEffect(HealOnKillEffectData effectData) : base(effectData)
    {
        //데이터 할당
        _data = effectData;

        //킬 카운트 초기화
        _currentKillCount = 0;
    }

    public override void Apply(Player player)
    {
        //플레이어의 킬 이벤트 등록
        player.OnKill += HandlePlayerKill;
    }

    public override void Remove(Player player)
    {
        //플레이어의 킬 이벤트 해제
        player.OnKill -= HandlePlayerKill;
    }

    private void HandlePlayerKill(PlayerDamageContext context)
    {
        //킬 카운트 증가
        _currentKillCount++;

        //목표 킬 카운트 미만일 시 패스
        if (_currentKillCount < _data.TargetKillCount) return;

        //킬 카운트 초기화
        _currentKillCount = 0;

        //플레이어 회복
        context.Player.PlayerHealth.Heal(_data.HealAmount);
    }

    public override string GetDescription()
    {
        if (_data.TargetKillCount <= 1)
        {
            //목표 처치 수가 1 이하일 시 기본 설명 반환
            return _data.GetDescription();
        }
        else
        {
            //목표 처치 수가 2 이상일 시 현재 kill count 포함
            return _data.GetDescription() + $"({_currentKillCount}/{_data.TargetKillCount})";
        }
    }
}