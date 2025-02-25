using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineStretch : Wire
    ,IDragHandler
    ,IEndDragHandler
{

    [SerializeField] private GameObject wire;
    [SerializeField] private Vector2 defaultSize; //초기 사이즈 값
    [SerializeField] private Vector2 defaultPosition; //초기 위치 값

    private Vector2 defaultWorldPosition;
    private RectTransform rectTransform;
    private bool isAnswer;
    private MiniGame_1 miniGame_1;

    public bool IsAnswer
    {
        get { return isAnswer; }
        set { isAnswer = value; }
    }

    public Vector2 DefaultPosition
    {
        get { return defaultPosition; }
        set { defaultPosition = value; }
    }

    public void SetMiniGame(MiniGame_1 game)
    {
        miniGame_1 = game;
    }

    private void Awake()
    {
        rectTransform = wire.GetComponent<RectTransform>();
        rectTransform.sizeDelta = defaultSize;
        rectTransform.anchoredPosition = defaultPosition;
    }
    public void OnDrag(PointerEventData eventData) //마우스 드래그 시 호출
    {
        defaultWorldPosition = rectTransform.parent.TransformPoint(defaultPosition);
        StretchBetweenPoints(defaultWorldPosition, Input.mousePosition);
    }
    public void OnEndDrag(PointerEventData eventData) //마우스 드래그 끝날 때 호출
    {
        if (isAnswer) //정답이면 멈춤
        {
            AudioManager.Instance.PlaySFX(5);
            miniGame_1.AnswerCorrectly();
            StretchBetweenPoints(defaultWorldPosition, defaultPosition);
            this.enabled = false;
        }
        else //정답이 아니면 원상 복구
        {
            AudioManager.Instance.PlaySFX(6);
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
