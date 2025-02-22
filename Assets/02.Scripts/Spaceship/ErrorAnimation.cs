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
            .AppendInterval(0.3f) // 0.3초 대기 (애니메이션 끝난 후)
            .AppendCallback(() => { spriteRenderer.enabled = false; })
            .AppendInterval(0.3f) // 0.3초 대기 (애니메이션 끝난 후)
            .SetLoops(-1, LoopType.Restart);
    }


    private void OnDisable()
    {
        sequence.Kill();
    }
}
