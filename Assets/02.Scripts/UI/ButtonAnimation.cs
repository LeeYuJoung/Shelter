using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector3 defaultSize = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 hoverSize = new Vector3(1.1f, 1.1f, 1.0f);
    [SerializeField] private Vector3 clickSize = new Vector3(0.9f, 0.9f, 1.0f);
    [SerializeField] private float durationTime = 0.1f;
    [SerializeField] private Ease ease = Ease.Linear;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    //마우스 올릴때
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(hoverSize, durationTime).SetEase(ease);
    }

    //마우스 내릴때
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(defaultSize, durationTime).SetEase(ease);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rect.DOScale(clickSize, durationTime).SetEase(ease);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect.DOScale(defaultSize, durationTime).SetEase(ease);
    }
}
