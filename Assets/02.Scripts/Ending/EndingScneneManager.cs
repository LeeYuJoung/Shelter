using Manager;
using UnityEngine;
using UnityEngine.UI;

public class EndingScneneManager : MonoBehaviour
{
    [Header("-----Data-----")]
    [SerializeField] private Sprite[] endingImageData;
    [SerializeField] private string[] endingTextData;

    [Header("-----EndingCard-----")]
    [SerializeField] private Image endingCardImage;
    [SerializeField] private Text endingCardText;
    void Start()
    {
        endingCardImage.sprite = endingImageData[(int)Ending.Type - 1];
        endingCardText.text = endingTextData[(int)Ending.Type - 1];
        AudioManager.Instance.EndingBGM((int)Ending.Type - 1);
    }
}