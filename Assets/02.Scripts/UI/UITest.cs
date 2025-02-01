using Donhyun.UI.Animation;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] UIInformation UIInfo;
    [SerializeField] AnimationType type;
    public void OpenUI()
    {
        UIAnimationManager.OpenUI(() => { gameObject.SetActive(true); }, UIInfo, type);
    }

    public void CloseUI()
    {
        UIAnimationManager.CloseUI(() => { gameObject.SetActive(false); }, UIInfo, type);
    }
}
