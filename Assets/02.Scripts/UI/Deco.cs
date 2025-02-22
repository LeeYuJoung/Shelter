using DG.Tweening;
using Donhyun.UI.Animation;
using UnityEngine;

public class Deco : MonoBehaviour
{
    [SerializeField] private UIInformation rocket;

    [SerializeField] private UIInformation sabotage1;

    [SerializeField] private UIInformation sabotage2;

    [SerializeField] private UIInformation settern;

    void Start()
    {
        rocket.rectTransform.DOAnchorPos(rocket.end, rocket.tweenDuration)
        .SetEase(rocket.ease)
        .SetLoops(-1, LoopType.Yoyo)
        .OnStart(() => { rocket.rectTransform.position = rocket.start; });

        sabotage1.rectTransform.DORotate(sabotage1.end, sabotage1.tweenDuration, RotateMode.LocalAxisAdd)
        .SetEase(sabotage1.ease)
        .SetLoops(-1);

        sabotage2.rectTransform.DORotate(sabotage2.end, sabotage2.tweenDuration, RotateMode.LocalAxisAdd)
        .SetEase(sabotage2.ease)
        .SetLoops(-1);

        settern.rectTransform.DOAnchorPos(settern.end, settern.tweenDuration)
        .SetEase(settern.ease)
        .SetLoops(-1, LoopType.Yoyo)
        .OnStart(() => { settern.rectTransform.position = settern.start; });
    }
}
