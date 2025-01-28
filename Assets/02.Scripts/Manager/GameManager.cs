using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        private int day = 7;
        private float dayTime = 90.0f;              // 하루 시간
        [SerializeField] private float currentTime; // 현재 시간

        [SerializeField] private int gold = 0;
        [SerializeField] private int collectionRoboyPiece = 1;
        [SerializeField] private int sweeperRobotPiece = 0;

        public int GetGold { get { return gold; } }
        public int GetcollectionRoboyPiece { get { return collectionRoboyPiece; } }
        public int GetsweeperRobotPiece { get { return sweeperRobotPiece; } }

        private bool isGameOver = false;

        private void Awake()
        {
            if (instance != null)
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
            if (isGameOver)
                return;

            Timmer();
        }

        // 초기화
        private void Init()
        {
            currentTime = dayTime;
        }

        // 시간 관리
        private void Timmer()
        {
            // 확인을 위해 20배 빠르게 시간이 흐르도록 함
            currentTime -= Time.deltaTime * 20;

            if(currentTime <= 0)
            {
                day--;
                currentTime = dayTime;
                UIManager.Instance.UpdateDayText(day);

                if(day <= 0)
                {
                    GameOver();
                }
            }
        }

        // 재화 사용
        public void UseGold(int useGold)
        {
            gold -= useGold;
            UIManager.Instance.UpdateGoldText(gold);
        }

        // 재화 획득
        public void GainGold(int getGold)
        {
            gold += getGold;
            UIManager.Instance.UpdateGoldText(gold);
        }

        // 로봇 관리
        public void RobotPiece(string robotType)
        {
            if(robotType == "CollectorRobot")
            {
                collectionRoboyPiece += 1;
            }
            else
            {
                sweeperRobotPiece += 1;
            }
        }

        // 게임 종료
        private void GameOver()
        {
            Debug.Log("::: Game Over :::");
            isGameOver = true;
            SceneChange(2);
        }

        // 씬 관리
        public void SceneChange(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        // 게임 나가기
        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}