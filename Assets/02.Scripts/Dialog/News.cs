using UnityEngine;
using UnityEngine.UI;

namespace yjlee.dialog
{
    public class News : MonoBehaviour
    {
        public DialogParse dialog;
        private DialogData[] talkDatas;
        public Text contextText;

        public string eventName;
        public int index = 0;

        private void Start()
        {
            dialog = GetComponent<DialogParse>();
            talkDatas = dialog.GetDialog(eventName);
            NextButton();
        }

        public virtual void NextButton()
        {
            // 대사가 null이 아니면 대사 출력
            if (talkDatas != null)
            {
                PrintDialog(talkDatas);
                index++;

                if (index >= talkDatas.Length)
                    index = 0;
            }
        }

        // 대화 정보를 출력
        public void PrintDialog(DialogData[] talkData)
        {
            foreach (string context in talkData[index].contexts)
            {
                contextText.text = context;
            }
        }
    }
}
