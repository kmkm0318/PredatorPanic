using UnityEngine;

/// <summary>
/// 진화 매니저 클래스
/// </summary>
public class EvolutionManager : MonoBehaviour
{
    #region 레퍼런스
    private MainMenuManager _mainMenuManager;
    #endregion

    #region 초기화
    public void Init(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;
    }
    #endregion

    #region 진화
    public bool TryUpgradeEvolution(EvolutionData evolutionData)
    {
        var userSaveDataManager = UserSaveDataManager.Instance;
        var userSaveData = userSaveDataManager.UserSaveData;

        //현재 진화 레벨 가져오기
        userSaveData.AcquiredEvolutions.Dictionary.TryGetValue(evolutionData.ID, out int currentLevel);

        //최대 레벨인지 확인
        if (currentLevel >= evolutionData.MaxLevel)
        {
            $"{evolutionData.Name}은(는) 이미 최대 레벨입니다.".LogWarning();
            return false;
        }

        //다음 레벨 가격 계산
        int nextLevel = currentLevel + 1;
        int price = evolutionData.GetPriceForLevel(nextLevel);

        //DNA 차감 시도
        if (!userSaveDataManager.TrySpendDNA(price))
        {
            "DNA가 부족합니다.".LogWarning();
            return false;
        }

        //진화 레벨 업데이트
        userSaveDataManager.UpdateEvolutionLevel(evolutionData.ID, nextLevel);

        //저장
        userSaveDataManager.SaveUserSaveData();

        return true;
    }
    #endregion
}