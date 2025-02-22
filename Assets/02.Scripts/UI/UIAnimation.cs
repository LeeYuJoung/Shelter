using Donhyun.UI.Animation;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] UIInformation UIInfo;
    [SerializeField] AnimationType type;
    private void OnEnable()
    {
        UIAnimationManager.OpenUI(() => { gameObject.SetActive(true); }, UIInfo, type);
    }

    public void Close()
    {
        UIAnimationManager.CloseUI(() => { transform.parent.gameObject.SetActive(false); }, UIInfo, type);
    }
}
