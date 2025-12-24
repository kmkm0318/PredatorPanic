using UnityEngine;

/// <summary>
/// 포인터 이벤트에 효과음을 재생하기 위한 UI 클래스
/// </summary>
public class PointerSfxPlayer : MonoBehaviour
{
    [SerializeField] private PointerHandler _pointerHandler;
    [SerializeField] private AudioData _enterSfxData;
    [SerializeField] private AudioData _exitSfxData;
    [SerializeField] private AudioData _downSfxData;
    [SerializeField] private AudioData _upSfxData;
    [SerializeField] private AudioData _clickSfxData;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _pointerHandler.OnPointerEntered += HandlePointerEntered;
        _pointerHandler.OnPointerExited += HandlePointerExited;
        _pointerHandler.OnPointerDowned += HandlePointerDowned;
        _pointerHandler.OnPointerUpped += HandlePointerUpped;
        _pointerHandler.OnPointerClicked += HandlePointerClicked;
    }

    private void HandlePointerEntered()
    {
        //데이터가 없으면 실행하지 않음
        if (_enterSfxData == null) return;

        //효과음 재생
        AudioManager.Instance.PlaySfx(_enterSfxData);
    }

    private void HandlePointerExited()
    {
        //데이터가 없으면 실행하지 않음
        if (_exitSfxData == null) return;

        //효과음 재생
        AudioManager.Instance.PlaySfx(_exitSfxData);
    }

    private void HandlePointerDowned()
    {
        //데이터가 없으면 실행하지 않음
        if (_downSfxData == null) return;

        //효과음 재생
        AudioManager.Instance.PlaySfx(_downSfxData);
    }

    private void HandlePointerUpped()
    {
        //데이터가 없으면 실행하지 않음
        if (_upSfxData == null) return;

        //효과음 재생
        AudioManager.Instance.PlaySfx(_upSfxData);
    }

    private void HandlePointerClicked()
    {
        //데이터가 없으면 실행하지 않음
        if (_clickSfxData == null) return;

        //효과음 재생
        AudioManager.Instance.PlaySfx(_clickSfxData);
    }
}