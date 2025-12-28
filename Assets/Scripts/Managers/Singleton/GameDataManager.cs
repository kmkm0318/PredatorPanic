/// <summary>
/// 게임 데이터 매니저 싱글톤 클래스
/// 메인 메뉴에서 선택한 플레이어, 무기 정보 등을 게임 씬까지 전달하는 역할
/// </summary>
public class GameDataManager : Singleton<GameDataManager>
{
    #region 선택한 데이터
    public PlayerData SelectedPlayerData { get; private set; }
    public WeaponData SelectedWeaponData { get; private set; }
    #endregion

    #region 선택 함수
    public void SetSelectedPlayerData(PlayerData playerData)
    {
        SelectedPlayerData = playerData;
    }

    public void SetSelectedWeaponData(WeaponData weaponData)
    {
        SelectedWeaponData = weaponData;
    }
    #endregion
}