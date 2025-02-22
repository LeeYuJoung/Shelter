using DG.Tweening;
using Donhyun.UI.Animation;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private UIInformation uiInfo;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        Sequence seq = DOTween.Sequence();

        rectTransform.localPosition = uiInfo.start;

        seq.SetAutoKill(false)
            .Append(rectTransform.DOAnchorPos(uiInfo.end, uiInfo.tweenDuration)
                                          .SetEase(uiInfo.ease)
                                          .SetUpdate(true))
           .Append(rectTransform.DOAnchorPos(uiInfo.start, uiInfo.tweenDuration)
                                          .SetEase(uiInfo.ease)
                                          .SetUpdate(true))
            .SetLoops(-1);

    }
}
