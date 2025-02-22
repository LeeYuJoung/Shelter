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
            .InsertCallback(0f, () => { spriteRenderer.enabled = true; }) // Tween 시작 전에 실행
            .Append(transform.DOScale(end, durationTime)  // 크기 확대 애니메이션
                .SetEase(ease)
            )
            .AppendInterval(0.3f) // 0.3초 대기 (애니메이션 끝난 후)
            .AppendCallback(() => {
                transform.localScale = start;
                spriteRenderer.enabled = false; // 애니메이션 끝난 후 숨김
            })
            .SetLoops(-1, LoopType.Restart);
    }


    private void OnDisable()
    {
        sequence.Kill();
    }
}
