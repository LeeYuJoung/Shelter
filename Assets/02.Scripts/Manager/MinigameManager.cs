using UnityEngine;

namespace Manager
{
    public class MinigameManager : MonoBehaviour
    {
        private static MinigameManager instance;
        public static MinigameManager Instance {  get { return instance; } }

        public GameObject[] machines;

        private float currentTime;
        private float errorTime;
        const float errorMinTime = 15.0f;
        const float errorMaxTime = 30.0f;

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
                currentTime = 0;
                errorTime = Random.Range(errorMinTime, errorMaxTime);

                OnError();
            }
        }

        private void Init()
        {
            currentTime = 0;
            errorTime = Random.Range(errorMinTime, errorMaxTime);
        }
        // 랜덤한 시간마다 각 기계에 에러 발생하도록 하기
        public void OnError()
        {

        }

        // 게임의 결과를 받아 그에 따른 이익 or 불이익 할당
        public void Result()
        {
            
        }
    }
}
