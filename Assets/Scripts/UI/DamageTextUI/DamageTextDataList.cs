using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지 텍스트 데이터 리스트
/// </summary>
[CreateAssetMenu(fileName = "DamageTextDataList", menuName = "SO/UI/DamageText/DamageTextDataList", order = 0)]
public class DamageTextDataList : ScriptableObject
{
    [Header("Damage Text Data List")]
    [field: SerializeField] public List<DamageTextData> DamageTextDatas { get; private set; }

    // 빠르게 찾기 위한 딕셔너리
    private Dictionary<DamageTextType, DamageTextData> _damageTextDataDict;
    public Dictionary<DamageTextType, DamageTextData> DamageTextDataDict
    {
        get
        {
            if (_damageTextDataDict == null)
            {
                _damageTextDataDict = new();
                foreach (var data in DamageTextDatas)
                {
                    _damageTextDataDict[data.DamageTextType] = data;
                }
            }
            return _damageTextDataDict;
        }
    }
}