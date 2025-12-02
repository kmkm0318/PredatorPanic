using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 비주얼 클래스
/// </summary>
public class EnemyVisual : MonoBehaviour
{
    // 히트 플래시 지속 시간
    private const float HIT_FLASH_DURATION = 0.1f;

    // 머티리얼 리스트
    private List<Material> _materials = new();

    #region 플래시 관련 변수
    int _FlashID = Shader.PropertyToID("_Flash");
    bool _isFlashing = false;
    float _flashTimer = 0f;
    #endregion

    private void Awake()
    {
        // 자식 오브젝트의 모든 렌더러에서 메터리얼 수집
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.materials)
            {
                _materials.Add(mat);
            }
        }
    }

    private void Update()
    {
        HandleFlash();
    }

    //플래시 처리 함수
    private void HandleFlash()
    {
        if (!_isFlashing) return;

        _flashTimer -= Time.deltaTime;
        if (_flashTimer <= 0f)
        {
            StopHitFlash();
        }
    }

    //플래시를 시작하는 함수
    public void StartHitFlash()
    {
        //플래시 중이 아닌 경우에 플래시 시작
        if (!_isFlashing)
        {
            _isFlashing = true;

            foreach (var mat in _materials)
            {
                mat.SetFloat(_FlashID, 1f);
            }
        }

        //타이머 초기화
        _flashTimer = HIT_FLASH_DURATION;
    }

    private void StopHitFlash()
    {
        // 플래시 종료
        _isFlashing = false;

        foreach (var mat in _materials)
        {
            mat.SetFloat(_FlashID, 0f);
        }
    }
}