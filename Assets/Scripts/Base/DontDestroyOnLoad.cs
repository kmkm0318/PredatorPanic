using UnityEngine;

/// <summary>
/// 씬 전환 시에도 파괴되지 않는 오브젝트를 위한 클래스
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}