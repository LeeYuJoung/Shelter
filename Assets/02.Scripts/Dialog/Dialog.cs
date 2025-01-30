using UnityEngine;

namespace yjlee.dialog
{
    [System.Serializable]
    public struct TalkData
    {
        public string name;        // 캐릭터 이름
        public string[] contexts;  // 대사 내용
    }

    public class Dialog : MonoBehaviour
    {
        // 대화 이벤트 이름
        [SerializeField] private string eventName;
        [SerializeField] private TalkData[] talkDatas;
    }
}
