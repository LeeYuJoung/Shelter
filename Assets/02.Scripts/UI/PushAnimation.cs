using DG.Tweening;
using Donhyun.UI.Animation;
using UnityEngine;

public class PushAnimation : MonoBehaviour
{
    [Header("-----Start는 초기, 마무리 크기, End는 눌릴 때 크기-----")]
    [SerializeField] private UIInformation button;

    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(button.rectTransform.DOScale(button.end, button.tweenDuration).SetEase(button.ease))
                .Append(button.rectTransform.DOScale(button.start, button.tweenDuration).SetEase(button.ease));
    }

    public void OnSuccess()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(button.rectTransform.DOScale(button.end, button.tweenDuration).SetEase(button.ease))
                .Append(button.rectTransform.DOScale(button.start, button.tweenDuration).SetEase(button.ease));
    }
}
