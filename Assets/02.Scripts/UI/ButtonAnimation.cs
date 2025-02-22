using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float hoverSize = 1.1f;
    [SerializeField] private float clickSize = 0.9f;
    [SerializeField] private float durationTime = 0.1f;
    [SerializeField] private Ease ease = Ease.Linear;

    private RectTransform rect;
    private Vector3 defaultSize;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        defaultSize = rect.localScale;
    }


    //마우스 올릴때
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(defaultSize * hoverSize, durationTime).SetEase(ease);
    }

    //마우스 내릴때
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(defaultSize, durationTime).SetEase(ease);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rect.DOScale(defaultSize * clickSize, durationTime).SetEase(ease);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect.DOScale(defaultSize, durationTime).SetEase(ease);
    }
}
