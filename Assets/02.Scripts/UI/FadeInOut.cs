using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOut : MonoBehaviour
{
    [Header("-----Fade Option (FadeIn이 밝아지는거)-----")]
    [SerializeField] private bool isFadeIn;
    [SerializeField] private float durationTime;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }


    private void OnEnable()
    {
        if(isFadeIn) //투명하게 변경
        {
            image.color = new Color32(0, 0, 0, 255);
            image.DOFade(0f, durationTime); 
        }
        else //불투명하게 변경
        {
            image.color = new Color32(0, 0, 0, 0);
            image.DOFade(1.0f, durationTime);
        }
    }

}
