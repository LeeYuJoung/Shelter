using System.Collections.Generic;
using System.Data;
using UnityEngine;
using yjlee.dialog;

namespace yjlee.dialog
{
    public class DialogParse : MonoBehaviour
    {
        [SerializeField] private TextAsset csvFile = null;
        private static Dictionary<string, TalkData[]> dialogDictionary = new Dictionary<string, TalkData[]>();

        public static TalkData[] GetDialog(string eventName)
        {
            return dialogDictionary[eventName];
        }

        public void SetTalkDictionary()
        {
            // 아래 한 줄 빼기
            string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
            // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string 배열에 줄 순서대로 저장
            string[] rows = csvText.Split(new char[] { '\n' });

            // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
            for(int i = 1; i < rows.Length; i++)
            {
                // 열을 쪼개서 배열에 담음
                string[] rowValues = rows[i].Split(new char[] { ' ' });

                // 유효한 이벤트 이름이 나올때까지 반복
                if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") 
                    continue;

                List<TalkData> talkDataList = new List<TalkData>();
                string eventName = rowValues[0];

                // talkDataList 하나를 만드는 반복문
                while (rowValues[0].Trim() != "end")
                {
                    List<string> contextList = new List<string>();
                }
            }
        }
    }
}
