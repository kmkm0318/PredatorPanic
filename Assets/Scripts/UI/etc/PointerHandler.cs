using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    #region 이벤트
    public event Action OnPointerEntered;
    public event Action OnPointerExited;
    public event Action OnPointerDowned;
    public event Action OnPointerUpped;
    public event Action OnPointerClicked;
    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExited?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDowned?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpped?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClicked?.Invoke();
    }
}