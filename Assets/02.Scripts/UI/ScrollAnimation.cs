using UnityEngine;
using DG.Tweening;

public class ScrollAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector2 defaultSize;
    [SerializeField] private Vector2 size;
    [SerializeField] private float durationTime;
    [SerializeField] private Ease ease;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        rect.sizeDelta = defaultSize;
        rect.DOSizeDelta(size, durationTime).SetEase(ease);
    }

    public void Close()
    {
        rect.DOSizeDelta(defaultSize, durationTime).SetEase(ease);
        gameObject.SetActive(false);
    }
}
