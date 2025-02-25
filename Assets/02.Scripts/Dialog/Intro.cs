using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using yjlee.dialog;
using Manager;
using UnityEngine.Audio;

namespace yjlee.intro
{
    public class Intro : MonoBehaviour
    {
        public DialogParse dialog;
        private DialogData[] talkDatas;

        public Button nextButton;
        public Text contextText;

        public string eventName;
        public int index = 0;

        private void Start()
        {
            dialog = GetComponent<DialogParse>();
            nextButton.onClick.AddListener(delegate { AudioManager.Instance.PlaySFX(0); });

            AudioManager.Instance.Init();
            AudioManager.Instance.PlayBGM(1);
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
                    SceneManager.LoadScene(2);
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
                contextText.DOText(context, 2.0f);
            }
        }
    }
}