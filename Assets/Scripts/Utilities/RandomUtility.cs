using UnityEngine;

/// <summary>
/// 랜덤 유틸리티 클래스
/// </summary>
public static class RandomUtility
{
    /// <summary>
    /// 주어진 확률 값에 따라 성공 여부를 반환합니다.
    /// </summary>
    public static bool ChanceTest(this float value)
    {
        //0 이하인 경우 항상 실패
        if (value <= 0f) return false;

        //1 이상인 경우 항상 성공
        if (value >= 1f) return true;

        //0과 1 사이의 값인 경우 랜덤 값과 비교하여 성공 여부 결정
        return Random.value < value;
    }
}