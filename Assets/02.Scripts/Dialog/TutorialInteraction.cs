using System;
using UnityEngine;
using UnityEngine.UI;

namespace yjlee.dialog
{
    public class TutorialInteraction : MonoBehaviour
    {
        private DialogParse dialog;
        public Text nameText;
        public Text contextText;

        public string eventName;
        private int index = 0;

        private void Start()
        {
            dialog = GetComponent<DialogParse>();
        }

        public void NextButton()
        {
            // 대화 정보 가져오기
            DialogData[] talkDatas = dialog.GetDialog(eventName);

            // 대사가 null이 아니면 대사 출력
            if(talkDatas != null)
            {
                PrintDialog(talkDatas);
                index++;
            }
        }

        // 대화 정보를 출력
        public void PrintDialog(DialogData[] talkData)
        {
            nameText.text = talkData[index].name;

            foreach (string context in talkData[index].contexts)
            {
                contextText.text = context;
            }
        }
    }
}