using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진화 데이터 스크립터블 오브젝트
/// 플레이어가 영구적으로 획득한 효과들을 정의합니다
/// </summary>
[CreateAssetMenu(fileName = "EvolutionData", menuName = "SO/Evolution/EvolutionData", order = 0)]
public class EvolutionData : ScriptableObject, IBasicData
{
    [Header("Basic Data")]
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private int _basePrice = 200;
    public string ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public Rarity Rarity => _rarity;
    public int BasePrice => _basePrice;

    [Header("Effect Data List")]
    [SerializeField] private List<EffectData> _effects = new();
    public List<EffectData> Effects => _effects;

    public string Getdescription()
    {
        // 스트링 리스트 생성
        List<string> effectDescriptions = new();

        // 각 이펙트의 설명을 리스트에 추가
        foreach (var effect in _effects)
        {
            effectDescriptions.Add(effect.Description);
        }

        // 리스트의 설명들을 개행 문자로 구분하여 하나의 문자열로 반환
        return string.Join("\n", effectDescriptions);
    }
}