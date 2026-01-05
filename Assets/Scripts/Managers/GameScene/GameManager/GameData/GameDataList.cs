using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataList", menuName = "SO/Game/GameDataList", order = 0)]
public class GameDataList : ScriptableObject
{
    [SerializeField] private List<GameData> _gameDatas;
    public List<GameData> GameDatas => _gameDatas;
}