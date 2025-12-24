using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총 무기 데이터 스크립터블 오브젝트. WeaponData 상속.
/// 총의 기본 정보, 사격 정보, 탄환 궤적 정보 포함
/// </summary>
[CreateAssetMenu(fileName = "GunData", menuName = "SO/Weapon/Gun/GunData", order = 0)]
public class GunData : WeaponData
{
    [Header("Gun Fire Data")]
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private TrailData _trailData;
    [SerializeField] private ExplosionData _explosionData;
    [SerializeField] private AudioData _fireSfxData;
    public BulletData BulletData => _bulletData;
    public TrailData TrailData => _trailData;
    public ExplosionData ExplosionData => _explosionData;
    public AudioData FireSfxData => _fireSfxData;

    [Header("Gun Stats")]
    [SerializeField] private List<StatEntity<GunStatType>> _initialStats;
    public List<StatEntity<GunStatType>> InitialStats => _initialStats;

    public override Weapon GetWeapon()
    {
        return new Gun(this);
    }

    public override string GetDescription()
    {
        // 총기 스탯 타입 데이터 딕셔너리 가져오기
        var dataDict = DataManager.Instance.GunStatTypeDataList.GunStatTypeDataDict;

        // 스탯 설명 생성
        List<string> descriptions = new();

        // 각 스탯에 대해 실행
        foreach (var stat in InitialStats)
        {
            if (dataDict.TryGetValue(stat.StatType, out GunStatTypeData statData))
            {
                // 스탯 타입 데이터가 존재하면 이름과 값 추가
                descriptions.Add($"{statData.StatName}: {stat.Value}");
            }
            else
            {
                // 스탯 타입 데이터가 없으면 기본 형식으로 추가
                descriptions.Add($"{stat.StatType}: {stat.Value}");
            }
        }

        // 결과 반환
        return string.Join("\n", descriptions);
    }
}