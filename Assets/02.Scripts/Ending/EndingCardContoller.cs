using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Donhyun.UI.Animation;

public class EndingCardContoller : MonoBehaviour
{
    [Header("-----EndingCards-----")]
    [SerializeField] private Image[] endingCards;
    [SerializeField] private UIInformation endingCardGroup;
    [SerializeField] private Vector2 defaultPosition;


    [Header("-----Button-----")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int currentCard = 0;
    public Sprite[] clearSprites;
    public Sprite lockSprite;
    //private Color32 clearColor = new Color32(255, 255, 255, 255);
    //private Color32 lockColor = new Color32(50, 50, 50, 255);

    private void Awake()
    {
        leftButton.onClick.AddListener(ClickLeftButton);
        rightButton.onClick.AddListener(ClickRightButton);

        for (int i = 0; i < endingCards.Length; i++)
        {
            endingCards[i].sprite = lockSprite;
        }
    }

    private void OnEnable()
    {
        currentCard = 0;
        endingCardGroup.rectTransform.localPosition = defaultPosition;

        foreach (EndingType type in Ending.SaveData.endingTypes)
        {
            if (type == EndingType.None) continue;
            Debug.Log(type);
            endingCards[(int)type - 1].sprite = clearSprites[(int)type - 1];
        }
    }

    //왼쪽 버튼 누를 시
    public void ClickLeftButton()
    {
        if (currentCard == 0) //최소에 도달
        {
            return;
        }

        if (DOTween.IsTweening(endingCardGroup.rectTransform)) //두트윈이 아직 실행중이면
        {
            return;
        }

        currentCard--;

        float target = endingCardGroup.rectTransform.localPosition.x + 1500.0f;

        endingCardGroup.rectTransform.DOAnchorPosX(target, endingCardGroup.tweenDuration).SetEase(endingCardGroup.ease);
    }

    //오른쪽 버튼 누를 시
    public void ClickRightButton()
    {
        if (currentCard == 6) //최대에 도달
        {
            return;
        }

        if (DOTween.IsTweening(endingCardGroup.rectTransform)) //두트윈이 아직 실행중이면
        {
            return;
        }

        currentCard++;

        float target = endingCardGroup.rectTransform.localPosition.x - 1500.0f;

        endingCardGroup.rectTransform.DOAnchorPosX(target, endingCardGroup.tweenDuration).SetEase(endingCardGroup.ease);

    }
}
