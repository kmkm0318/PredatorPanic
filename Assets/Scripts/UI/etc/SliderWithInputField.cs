using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 슬라이더와 입력 필드가 결합된 UI 컴포넌트
/// </summary>
public class SliderWithInputField : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_InputField _inputField;

    private bool _isUpdating = false;

    private void Awake()
    {
        //이벤트 등록
        _slider.onValueChanged.AddListener(HandleOnSliderValueChanged);
        _inputField.onEndEdit.AddListener(HandleOnInputFieldEndEdit);
    }

    private void OnEnable()
    {
        //초기 값 동기화
        _isUpdating = true;
        _inputField.text = _slider.value.ToString("0.##");
        _isUpdating = false;
    }

    private void HandleOnSliderValueChanged(float value)
    {
        //업데이트 중이면 패스
        if (_isUpdating) return;
        _isUpdating = true;

        //입력 필드 값 갱신
        _inputField.text = value.ToString("0.##");

        _isUpdating = false;
    }

    private void HandleOnInputFieldEndEdit(string str)
    {
        //업데이트 중이면 패스
        if (_isUpdating) return;
        _isUpdating = true;

        //문자열을 float로 변환 시도
        if (float.TryParse(str, out float value))
        {
            //슬라이더 값 갱신
            _slider.value = value;

            //입력 필드 값 갱신
            _inputField.text = _slider.value.ToString("0.##");
        }
        else
        {
            //변환 실패 시 입력 필드 값을 슬라이더 값으로 되돌림
            _inputField.text = _slider.value.ToString("0.##");
        }

        _isUpdating = false;
    }
}