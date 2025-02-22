using Donhyun.UI.Animation;
using UnityEngine;
using DG.Tweening;

public class StickAnimation : MonoBehaviour
{
    [SerializeField] private UIInformation stick;

    private void OnEnable()
    {
        stick.rectTransform.localPosition = stick.start;
        stick.rectTransform.DOAnchorPos(stick.end, stick.tweenDuration).SetEase(stick.ease);
    }

    public void Close()
    {
        stick.rectTransform.DOAnchorPos(stick.start, stick.tweenDuration).SetEase(stick.ease);
    }
}
