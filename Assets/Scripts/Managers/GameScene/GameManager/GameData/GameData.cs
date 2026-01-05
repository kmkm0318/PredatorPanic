using UnityEngine;

/// <summary>
/// 게임 매니저의 데이터 클래스
/// 라운드 시간과 목표 라운드 등 게임 진행에 필요한 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "GameData", menuName = "SO/Game/GameData", order = 0)]
public class GameData : ScriptableObject
{
    [Header("Round Settings")]
    [SerializeField] private float _roundStartDelay = 3f;
    [SerializeField] private float _roundDuration = 60f;
    public float RoundStartDelay => _roundStartDelay;
    public float RoundDuration => _roundDuration;

    [Header("BGM Audio Data")]
    [SerializeField] private AudioData _gamePlayingBGMAudioData;
    [SerializeField] private AudioData _gameClearBGMAudioData;
    [SerializeField] private AudioData _gameOverBGMAudioData;
    public AudioData GamePlayingBGMAudioData => _gamePlayingBGMAudioData;
    public AudioData GameClearBGMAudioData => _gameClearBGMAudioData;
    public AudioData GameOverBGMAudioData => _gameOverBGMAudioData;
}