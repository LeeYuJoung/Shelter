using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace yjlee.dialog
{
    public class TutorialController : MonoBehaviour
    {
        private static TutorialController instance;
        public static TutorialController Instance {  get { return instance; } }

        public DialogParse dialog;
        private DialogData[] talkDatas;

        public GameObject dialogObject;
        public GameObject[] news;
        public Text contextText;

        public string eventName;
        public int index = 0;
        public int dday = 5;

        private void Awake()
        {
            if(instance != null )
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            dialog = GetComponent<DialogParse>();
            Init();
        }

        public void Init()
        {
            for (int i = 0; i < news.Length; i++)
            {
                news[i].SetActive(false);
            }

            eventName = "D" + dday;
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
                    dday--;
                    index = 0;
                    dialogObject.SetActive(false);

                    for (int i = 0; i < news.Length; i++)
                    {
                        news[i].SetActive(true);
                    }

                    if(dday != -1)
                    {
                        GameManager.Instance.GameStart();
                        return;
                    }
                    else
                    {

                    }
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