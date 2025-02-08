using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using yjlee.robot;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        public Sprite[] backgrounds;
        public SpriteRenderer bgRenderer;

        public GameObject collectorRobotPrefab;
        public GameObject sweeperRobotPrefab;

        public List<GameObject> collectorRobots;
        public List<GameObject> sweeperRobots;
        public int collectorRobotLevel;
        public int sweeperRobotLevel;

        private int day = 0;
        private int dayRange = 1;
        private float dayTime = 90.0f;              // 하루 시간
        [SerializeField] private float currentTime; // 현재 시간

        [SerializeField] private int gold = 0;
        public int GetGold { get { return gold; } }

        public bool isRadeRoomUnLock = false;
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
            currentTime = 0;
            collectorRobotLevel = 1;
            sweeperRobotLevel = 1;
        }

        // 시간 관리
        private void Timmer()
        {
            currentTime += Time.deltaTime;

            if(currentTime >= dayTime)
            {
                day++;
                dayRange = 1;
                currentTime = 0;

                ChangeBackground();
                UIManager.Instance.UpdateDayImage(day);
                UIManager.Instance.UpdateTimeImage(0);

                if (day >= 7)
                {
                    GameOver();
                }
            }
            else
            {
                if(currentTime >= 18.0f * dayRange)
                {
                    UIManager.Instance.UpdateTimeImage(dayRange);
                    dayRange++;
                }
            }
        }

        // 배경 관리
        public void ChangeBackground()
        {
            if((day + 1) % 2 != 0)
            {
                bgRenderer.sprite = backgrounds[(day == 2) ? (1) : (day == 4) ? (2) : 3];
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

        // 로봇 수량 관리
        public void RobotPiece(string robotType)
        {
            if(robotType == "CollectorRobot")
            {
                GameObject robot = Instantiate(collectorRobotPrefab, Vector2.zero, Quaternion.identity);
                collectorRobots.Add(robot);
            }
            else
            {
                GameObject robot = Instantiate(sweeperRobotPrefab, Vector2.zero, Quaternion.identity);
                sweeperRobots.Add(robot);
            }

            UIManager.Instance.UpdateRobotPieceText(collectorRobots.Count, sweeperRobots.Count);
        }

        // 로봇 능력치 관리
        public void RobotStatusUp(string robotType)
        {
            if(robotType == "CollectorRobot")
            {
                for (int i = 0; i < collectorRobots.Count; i++)
                {
                    Robotcontroller robotController = collectorRobots[i].GetComponent<Robotcontroller>();

                    if (robotController != null)
                    {
                        robotController.moveSpeed += 10;
                        robotController.workTime -= 0.5f;
                        robotController.breakTime -= 0.5f;
                        robotController.SpeedInit();
                    }
                }
            }
            else
            {
                for (int i = 0; i < sweeperRobots.Count; i++)
                {
                    Robotcontroller robotController = sweeperRobots[i].GetComponent<Robotcontroller>();

                    if (robotController != null)
                    {
                        robotController.moveSpeed += 10;
                        robotController.workTime -= 0.5f;
                        robotController.breakTime -= 0.5f;
                        robotController.SpeedInit();
                    }
                }
            }
        }

        // 해상도 관리
        public void Resolution()
        {

        }

        // 게임 정지
        public void GameStop(bool isStop)
        {
            Time.timeScale = (isStop) ? 0 : 1;
        }

        // 게임 종료
        private void GameOver()
        {
            Debug.Log("::: Game Over :::");
            isGameOver = true;
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