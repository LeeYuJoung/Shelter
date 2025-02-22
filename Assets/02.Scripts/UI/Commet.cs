using DG.Tweening;
using Donhyun.UI.Animation;
using UnityEngine;

public class Commet : MonoBehaviour
{
    [SerializeField] private UIInformation uiInfo;
    [SerializeField] private float delay;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        Sequence seq = DOTween.Sequence();

        rectTransform.localScale = uiInfo.start;
        rectTransform.pivot = new Vector2(1.0f, 0.5f);

        seq.SetAutoKill(false)
            .AppendInterval(Random.Range(0.0f, delay))
            .Append(rectTransform.DOScaleX(uiInfo.end.x, uiInfo.tweenDuration)
                                          .SetEase(uiInfo.ease)
                                          .SetUpdate(true)
                                          .OnComplete(() =>
                                          {
                                              rectTransform.localPosition = new Vector2(rectTransform.localPosition.x - rectTransform.rect.width, rectTransform.localPosition.y);
                                              rectTransform.pivot = new Vector2(0.0f, 0.5f);
                                          }))
           .Append(rectTransform.DOScaleX(uiInfo.start.x, uiInfo.tweenDuration)
                                          .SetEase(uiInfo.ease)
                                          .SetUpdate(true)
                                          .OnComplete(() =>
                                          {
                                              rectTransform.localPosition = new Vector2(rectTransform.localPosition.x + rectTransform.rect.width, rectTransform.localPosition.y);
                                              rectTransform.pivot = new Vector2(1.0f, 0.5f);
                                          }))
            .SetLoops(-1);

    }
}
