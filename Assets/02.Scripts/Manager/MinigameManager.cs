using UnityEngine;

namespace Manager
{
    public class MinigameManager : MonoBehaviour
    {
        private static MinigameManager instance;
        public static MinigameManager Instance {  get { return instance; } }

        public GameObject[] machines;
        private bool[] isPlaying;

        private float currentTime;
        private float errorTime;
        const float errorMinTime = 15.0f;
        const float errorMaxTime = 30.0f;

        private float gamePlayTime = 15.0f;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }

            Init();
        }

        private void Update()
        {
            currentTime += Time.deltaTime;

            if( currentTime >= errorTime )
            {
                //OnError();
            }
        }

        private void Init()
        {
            currentTime = 0;
            errorTime = Random.Range(errorMinTime, errorMaxTime);
            isPlaying = new bool[3] { false, false, false };
        }

        // 랜덤한 시간마다 각 기계에 에러 발생하도록 하기
        public void OnError()
        {
            int gameIndex = Random.Range(0, machines.Length);

            if (!isPlaying[gameIndex])
            {
                currentTime = 0;
                errorTime = Random.Range(errorMinTime, errorMaxTime);

                isPlaying[gameIndex] = true;
                machines[gameIndex].SetActive(true);
            }
        }

        // 에러 버튼 클릭 시 각 기계에 해당하는 게임 실행
        public void GameStart()
        {

        }

        // 게임의 결과를 받아 그에 따라 스테이터스 수치 변동
        public void GameResult(bool isClear)
        {
            if(isClear)
            {

            }
            else
            {

            }
        }
    }
}
