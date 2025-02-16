using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace yjlee.dialog
{
    public class TutorialInteraction : MonoBehaviour
    {
        public DialogParse dialog;
        private DialogData[] talkDatas;

        public GameObject dialogObject;
        public GameObject[] news;
        public Text contextText;

        public string eventName;
        public int index = 0;

        private void Start()
        {
            dialog = GetComponent<DialogParse>();

            dialogObject.SetActive(true);
            talkDatas = dialog.GetDialog(eventName);
            NextButton();
        }

        public virtual void NextButton()
        {
            contextText.DOKill();
            contextText.text = "";

            // 대사가 null이 아니면 대사 출력
            if (talkDatas != null)
            {
                if (index >= talkDatas.Length)
                {
                    dialogObject.SetActive(false);

                    for(int i = 0; i < news.Length; i++)
                    {
                        news[i].SetActive(true);
                    }

                    GameManager.Instance.GameStart();
                    return;
                }

                PrintDialog(talkDatas);
                index++;
            }
        }

        // 대화 정보를 출력
        public void PrintDialog(DialogData[] talkData)
        {
            foreach (string context in talkData[index].contexts)
            {
                contextText.DOText(context, 2.5f);
            }
        }
    }
}