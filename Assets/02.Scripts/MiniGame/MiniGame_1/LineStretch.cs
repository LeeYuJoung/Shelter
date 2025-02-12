using UnityEngine;
using UnityEngine.EventSystems;

public class LineStretch : Wire
    ,IDragHandler
    ,IEndDragHandler
{
    [SerializeField] private Vector2 defaultSize; //초기 사이즈 값
    [SerializeField] private Vector2 defaultPosition; //초기 위치 값

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = defaultSize;
        rectTransform.anchoredPosition = defaultPosition;
    }
    public void OnDrag(PointerEventData eventData) //마우스 드래그 시 호출
    {
        Vector2 defaultWorldPosition = rectTransform.parent.TransformPoint(defaultPosition);
        StretchBetweenPoints(defaultWorldPosition, Input.mousePosition);
    }
    public void OnEndDrag(PointerEventData eventData) //마우스 드래그 끝날 때 호출
    {
        if(IsAnswer)
        {

        }
        else
        {
            rectTransform.sizeDelta = defaultSize;
            rectTransform.anchoredPosition = defaultPosition;
            rectTransform.rotation = Quaternion.identity;
        }
    }

    void StretchBetweenPoints(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        if (distance < 25.0f || end.x < start.x) return;
        rectTransform.sizeDelta = new Vector2(distance, rectTransform.sizeDelta.y);

        Debug.Log("시작좌표 : " + start);
        Debug.Log("끝 좌표 : " + end);

        Vector2 direction = end - start;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 회전 적용
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
