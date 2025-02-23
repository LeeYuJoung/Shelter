using System.Collections;
using UnityEngine;
using EnumTypes;
using Manager;
using UnityEngine.Audio;

namespace yjlee.robot
{
    public class Robotcontroller : MonoBehaviour
    {
        private Rigidbody2D robotRigidbody;
        private AudioSource audioSource;
        private Animator robotAnimator;
        private PathFinding pathFinding;

        public Robot robot;
        public GameObject target;

        public GameObject[] sweeperPos;
        public int index = 0;

        public RobotState robotState;
        public float moveSpeed;
        public float workTime;
        public float breakTime;
        public int getGold;
        public float range;

        public float currentTime;
        public bool isPickUp = false;  // 수집 로봇만 사용

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (GameManager.Instance.isGameOver)
            {
                pathFinding.moveSpeed = 0.0f;
                return;
            }
            else
                pathFinding.moveSpeed = moveSpeed;

            switch (robotState)
            {
                case RobotState.Idel:
                    robotState = (GameManager.Instance.isGameOver) ? RobotState.Idel : RobotState.Search;
                    break;
                case RobotState.Search:
                    TargetSearch();
                    break;
                case RobotState.Move:
                    Move(false);
                    break;
                case RobotState.PickUpMove:
                    Move(true);
                    break;
                case RobotState.Work:
                    Work();
                    break;
                case RobotState.Drop:
                    break;
                case RobotState.Breaking:
                    Break();
                    break;
            }
        }

        private void Init()
        {
            robotRigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            robotAnimator = GetComponentInChildren<Animator>();
            pathFinding = GetComponent<PathFinding>();

            robotState = RobotState.Idel;
            //moveSpeed = 60.0f;
            getGold = 5;
            workTime = robot.workTime;
            breakTime = robot.breakTime;
            range = robot.range;

            currentTime = 0;
        }

        // a* 알고리즘 이동 속도 재설정
        public void SpeedInit()
        {
            pathFinding.moveSpeed = moveSpeed;
        }

        #region 목적지 설정
        public void TargetSearch()
        {
            if (robot.robotType == RobotType.Collector)
            {
                // 수집 로봇이라면 랜덤한 외부 목적지와 쓰레기 소각장을 번갈아 가며 목적지로 할당받고 이동
                if (!isPickUp)
                {
                    GameObject[] targets = GameObject.FindGameObjectsWithTag("Destination");

                    if(targets.Length > 0)
                    {
                        target = targets[Random.Range(0, targets.Length)];
                        pathFinding.target = target;

                        robotState = RobotState.Move;
                        pathFinding.walkable = true;
                    }
                }
                else
                {
                    target = GameObject.FindGameObjectWithTag(robot.targetName);
                    pathFinding.target = target;

                    robotState = RobotState.PickUpMove;
                    pathFinding.walkable = true;
                }
            }
            else if(robot.robotType == RobotType.Sweeper)
            {
                sweeperPos = GameObject.FindGameObjectsWithTag("PartDestination");
                int _index = Random.Range(0, sweeperPos.Length);

                if(_index != index && !GameManager.Instance.isCleaning[_index])
                {
                    index = _index;
                    GameManager.Instance.isCleaning[_index] = true;
                    target = sweeperPos[_index];
                    pathFinding.target = target;

                    robotState = RobotState.Move;
                    pathFinding.walkable = true;
                }
                else
                {
                    robotState = RobotState.Search;
                    pathFinding.walkable = false;
                }
            }
        }
        #endregion

        #region 이동 실행
        public void Move(bool isPickUp)
        {
            if (!isPickUp)
            {
                robotAnimator.SetBool("Move", true);
                SetAnimator("MoveX", "MoveY", pathFinding.dir);
            }
            else
            {
                SetAnimator("PickUpX", "PickUpY", pathFinding.dir);
            }
        }
        #endregion

        #region 작업 실행
        public void Work()
        {
            currentTime += Time.deltaTime;

            if (robot.robotType == RobotType.Collector)
            {
                if (currentTime >= workTime)
                {
                    currentTime = 0;
                    robotState = RobotState.Search;
                }
            }
            else if (robot.robotType == RobotType.Sweeper)
            {
                if (currentTime >= workTime)
                {
                    currentTime = 0;
                    audioSource.Stop();
                    robotState = RobotState.Breaking;
                    GameManager.Instance.GainGold(getGold);
                }
            }
        }
        #endregion

        #region 청소 로봇 휴식 실행
        public void Break()
        {
            currentTime += Time.deltaTime;
            robotAnimator.SetBool("Clean", false);
            robotAnimator.SetBool("Move", false);

            if (currentTime >= breakTime)
            {
                currentTime = 0;
                GameManager.Instance.isCleaning[index] = false;
                robotState = RobotState.Search;
            }
        }
        #endregion

        #region 수집 로봇 내려놓기 실행
        public IEnumerator Drop()
        {
            StatusManager.Instance.FuelGaugeChange();
            robotAnimator.SetBool("isPickUp", isPickUp);
            robotAnimator.SetTrigger("Drop");

            yield return new WaitForSeconds(1.0f);

            robotState = RobotState.Search;
        }
        #endregion

        // 애니메이션 조절
        public void SetAnimator(string name1, string name2, Vector2 dir)
        {
            Vector2 setDir = dir.normalized;

            setDir.x = (setDir.x >= 0.5) ? 1 : (setDir.x <= -0.5) ? -1 : 0;
            setDir.y = (setDir.y >= 0.5) ? 1 : (setDir.y <= -0.5) ? -1 : 0;

            robotAnimator.SetFloat(name1, setDir.x);
            robotAnimator.SetFloat(name2, setDir.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Destination") && robot.robotType == RobotType.Collector)
            {
                isPickUp = true;
                robotState = RobotState.Work;
                pathFinding.walkable = false;
                robotAnimator.SetBool("isPickUp", isPickUp);
            }
            else if (collision.collider.CompareTag(robot.targetName) && robot.robotType == RobotType.Collector)
            {
                if (robotState != RobotState.PickUpMove)
                    return;

                isPickUp = false;
                pathFinding.walkable = false;
                robotState = RobotState.Drop;
                StartCoroutine(Drop());
            }
            else if (collision.collider.CompareTag("PartDestination") && robot.robotType == RobotType.Sweeper)
            {
                robotRigidbody.linearVelocity = Vector3.zero;
                robotRigidbody.angularVelocity = 0.0f;

                audioSource.Play();
                robotState = RobotState.Work;
                pathFinding.walkable = false;
                robotAnimator.SetBool("Clean", true);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            robotRigidbody.linearVelocity = Vector3.zero;
            robotRigidbody.angularVelocity = 0.0f;
        }
    }
}