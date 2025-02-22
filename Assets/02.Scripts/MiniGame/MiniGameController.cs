using System.Collections;
using UnityEngine;

namespace MiniGame
{
    public class MiniGameController : MonoBehaviour
    {
        public float playTime = 15.0f;
        public int plusPoint = 20;
        public int minusPoint = 10;
        public int reward = 5;
        public int penalty = 5;

        protected float currentTime = 0;
        protected float beepTime = 10.0f;

        public bool isError = false;
        public bool isPlaying = false;

        // 게임 시작
        public virtual void GameStart()
        {
            isPlaying = true;
        }

        // 게임 종료
        public virtual void ClearGame()
        {
            isError = false;
            isPlaying = false;
        }

        // 게임 수치 상승
        public virtual void GameLevelUp()
        {
            playTime -= 2.0f;
            plusPoint -= 2;
            minusPoint += 2;
        }

        // 경보 발생
        public virtual void OnBeep()
        {
            Debug.Log(":: Beep ::");
        }

        // 보상 획득
        public virtual void GetReward()
        {

        }

        // 패널티 획득
        public virtual void GetPenalty()
        {

        }

        // 게임 강제 종료
        public virtual void ForcingGameOver()
        {

        }
    }
}