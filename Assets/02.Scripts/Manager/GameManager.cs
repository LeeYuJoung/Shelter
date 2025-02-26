using DG.Tweening;
using MiniGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using yjlee.dialog;
using yjlee.robot;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        public Ending ending;
        public Status status;

        public List<Resolution> resolutions = new List<Resolution>();
        public int optimalResolutionIndex = 0;

        public Sprite[] backgrounds;
        public SpriteRenderer bgRenderer;

        public GameObject[] machines;

        public GameObject collectorRobotPrefab;
        public GameObject sweeperRobotPrefab;

        public List<GameObject> collectorRobots;
        public List<GameObject> sweeperRobots;
        public int collectorRobotLevel;
        public int sweeperRobotLevel;

        public Button startImage;

        private int day = 0;
        private int dayRange = 1;
        private float dayTime = 100.0f;              // 하루 시간
        [SerializeField] private float currentTime;  // 현재 시간

        [SerializeField] private int gold = 0;
        public int GetGold { get { return gold; } }

        public bool[] isCleaning = new bool[4] { false, false, false, false };
        public bool isRadeRoomUnLock = false;
        public bool isGameOver = true;
        public bool isTakeOff = false;

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
            Application.targetFrameRate = 65;
        }

        private void Start()
        {
            Screen.SetResolution(resolutions[AudioManager.Instance.resolutionIndex].width, resolutions[AudioManager.Instance.resolutionIndex].height, Screen.fullScreen);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

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

            resolutions.Add(new Resolution { width = 2560, height = 1440 });
            resolutions.Add(new Resolution { width = 1920, height = 1080 });
            resolutions.Add(new Resolution { width = 1280, height = 720 });
        }

        // 시간 관리
        private void Timmer()
        {
            currentTime += Time.deltaTime;

            if (day >= 5)
            {
                isGameOver = true;
            }

            if (currentTime >= dayTime)
            {
                day++;
                dayRange = 1;
                currentTime = 0;
                isGameOver = true;

                UIManager.Instance.AllCLose();
                ChangeBackground();
                MiniGameManager.Instance.AllMiniGameStop();
                UIManager.Instance.UpdateDayImage(day);
                UIManager.Instance.UpdateTimeImage(0);
                TutorialController.Instance.Init();

                // 미니게임 난이도 상승
                for (int i = 0; i < machines.Length; i++)
                {
                    machines[i].GetComponent<MiniGameController>().GameLevelUp();
                }
            }
            else
            {
                if(currentTime >= 20.0f * dayRange)
                {
                    UIManager.Instance.UpdateTimeImage(dayRange);
                    dayRange++;
                }
            }
        }

        // 배경 관리
        public void ChangeBackground()
        {
            bgRenderer.sprite = backgrounds[day - 1];
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
        public void RobotPiece(int robotType)
        {
            if(robotType == 1)
            {
                GameObject robot = Instantiate(collectorRobotPrefab, new Vector2(5.0f, -1.25f), Quaternion.identity);
                collectorRobots.Add(robot);
            }
            else
            {
                GameObject robot = Instantiate(sweeperRobotPrefab, new Vector2(5.0f, -1.25f), Quaternion.identity);
                sweeperRobots.Add(robot);
            }

            UIManager.Instance.UpdateRobotPieceText(collectorRobots.Count, sweeperRobots.Count);
        }

        // 로봇 능력치 관리
        public void RobotStatusUp(int robotType)
        {
            if(robotType == 1)
            {
                for (int i = 0; i < collectorRobots.Count; i++)
                {
                    Robotcontroller robotController = collectorRobots[i].GetComponent<Robotcontroller>();

                    if (robotController != null)
                    {
                        robotController.moveSpeed += 5;
                        robotController.workTime -= 2.0f;
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
                        robotController.moveSpeed += 5;
                        robotController.getGold += 5;
                        robotController.workTime -= 2.0f;
                        robotController.breakTime -= 2.0f;
                        robotController.SpeedInit();
                    }
                }
            }
        }

        // 게임 시작
        public void GameStart()
        {
            isGameOver = false;
        }

        // 게임 정지
        public void GameStop(bool isStop)
        {
            Time.timeScale = (isStop) ? 0 : 1;
        }

        // 게임 종료
        public void GameOver()
        {
            StartCoroutine(IGameOver());
        }

        IEnumerator IGameOver()
        {
            if (!isTakeOff)
            {
                isTakeOff = true;
                AudioManager.Instance.PlaySFX(12);
                startImage.GetComponent<RectTransform>().DOAnchorPosY(-208, 0.5f).OnComplete(delegate { startImage.GetComponent<RectTransform>().DOAnchorPosY(-190, 0.5f); });

                yield return new WaitForSeconds(4f);
                ending.ExcuteEnding(StatusManager.Instance.status.statusData);
            }
        }

        // 씬 관리
        public void SceneChange(int sceneIndex)
        {
            if(sceneIndex == 0)
            {
                AudioManager.Instance.PlayBGM(0);
                SceneManager.LoadScene(sceneIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }

        // 스테이터스 데이터 초기화
        public void StatusClear()
        {
            status.statusData.FuelAmount = 0;
            status.statusData.HullRestorationRate = 0;
            status.statusData.MotorRestorationRate = 0;
            status.statusData.EngineRestorationRate = 0;
            status.statusData.RadarRestorationRate = 0;
            status.statusData.RadarOutputAmount = 0;
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