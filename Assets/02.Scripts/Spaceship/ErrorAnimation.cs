using DG.Tweening;
using UnityEngine;

public class ErrorAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private Ease ease;
    [SerializeField] private float durationTime;

    Sequence sequence;

    private void OnEnable()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        sequence = DOTween.Sequence();

        sequence
            .AppendCallback(() => {
                transform.localScale = start;
                spriteRenderer.enabled = true;
            })
            .Append(transform.DOScale(end, durationTime)
                .SetEase(ease))
            .SetLoops(-1, LoopType.Yoyo);
    }


    private void OnDisable()
    {
        sequence.Kill();
    }
}
