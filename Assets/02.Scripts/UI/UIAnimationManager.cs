using System;
using DG.Tweening;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

namespace Donhyun.UI.Animation
{
    [Serializable]
    public struct UIInformation
    {
        public RectTransform rectTransform;
        public Vector2 start, end;
        public float tweenDuration;
    }

    public enum AnimationType
    {
        Slide,
        PopUp
    }

    public class UIAnimationManager
    {
        public static void OpenUI(Action callBack, UIInformation UIInfo, AnimationType type)
        {
            Init(UIInfo, type);
            switch (type)
            {
                case AnimationType.Slide:
                    SlideAnimation(callBack, false, UIInfo);
                    break;
                case AnimationType.PopUp:
                    PopUpAnimation(callBack, false, UIInfo);
                    break;
            }
        }
        public static void CloseUI(Action callBack, UIInformation UIInfo, AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Slide:
                    SlideAnimation(callBack, true, UIInfo);
                    break;
                case AnimationType.PopUp:
                    PopUpAnimation(callBack, true, UIInfo);
                    break;
            }
        }


        private static void SlideAnimation(Action callBack, bool reverse, UIInformation UIInfo)
        {
            if (!reverse)
            {
                Debug.Log("슬라이드 애니메이션 실행 - 열기");
                callBack();
                UIInfo.rectTransform.DOAnchorPos(UIInfo.end, UIInfo.tweenDuration)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);
            }
            else
            {
                Debug.Log("슬라이드 애니메이션 실행 - 닫기");
                UIInfo.rectTransform.DOAnchorPos(UIInfo.start, UIInfo.tweenDuration)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .OnComplete(() => { callBack(); });
            }
        }

        private static void PopUpAnimation(Action callBack, bool reverse, UIInformation UIInfo)
        {
            if (!reverse)
            {
                Debug.Log("팝업 애니메이션 실행 - 열기");
                callBack();
                UIInfo.rectTransform.DOScale(UIInfo.end, UIInfo.tweenDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);
            }
            else
            {
                Debug.Log("팝업 애니메이션 실행 - 닫기");
                UIInfo.rectTransform.DOScale(UIInfo.start, UIInfo.tweenDuration)
                    .SetEase(Ease.InBack)
                    .SetUpdate(true)
                    .OnComplete(() => { callBack(); });
            }
        }


        private static void Init(UIInformation UIInfo, AnimationType type)
        {
            Debug.Log("UIAnimation - 초기값 조정");
            switch (type)
            {
                case AnimationType.Slide:
                    UIInfo.rectTransform.localPosition = new Vector3(UIInfo.start.x, UIInfo.start.y, UIInfo.rectTransform.localPosition.z);
                    break;
                case AnimationType.PopUp:
                    UIInfo.rectTransform.localScale = new Vector3(UIInfo.start.x, UIInfo.start.y, UIInfo.rectTransform.localScale.z);
                    break;
            }
        }
    }
}
