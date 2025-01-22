using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

namespace yjlee.robot
{
    public class RobotController : MonoBehaviour
    {
        private Animator robotAnimator;
        private PathFinding pathFinding;

        public Robot robot;
        public GameObject target;
        public Transform[] positons;

        public RobotState robotState;
        public float moveSpeed;
        public float workTime;
        public float breakTime;
        public float range;

        public float currentTime;
        public bool isPickUp = false;  // 수집 로봇만 사용

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            switch (robotState)
            {
                case RobotState.Idel:
                    TargetSetting();
                    break;
                case RobotState.Move:
                    break;
                case RobotState.Work:
                    Work();
                    break;
                case RobotState.breaking:
                    Break();
                    break;
            }
        }

        private void Init()
        {
            robotAnimator = GetComponent<Animator>();
            pathFinding = GetComponent<PathFinding>();
            positons = GameObject.Find("Positions").GetComponentsInChildren<Transform>();

            robotState = RobotState.Idel;
            moveSpeed = robot.moveSpeed;
            workTime = robot.workTime;
            breakTime = robot.breakTime;
            range = robot.range;

            currentTime = 0;
        }

        #region 목적지 설정
        public void TargetSetting()
        {
            if (robot.robotType == RobotType.Collector)
            {
                // 수집 로봇이라면 랜덤한 외부 목적지와 쓰레기 소각장을 번갈아 가며 목적지로 할당받고 이동
                if (!isPickUp)
                {
                    target = positons[Random.Range(1, positons.Length)].gameObject;
                    pathFinding.target = target;

                    robotState = RobotState.Move;
                    pathFinding.walkable = true;
                }
                else
                {
                    target = GameObject.FindGameObjectWithTag(robot.targetName);
                    pathFinding.target = target;

                    robotState = RobotState.Move;
                    pathFinding.walkable = true;
                }
            }
            else if(robot.robotType == RobotType.Sweeper)
            {
                // 청소 로봇이라면 씬 내에 오물이 있을 경우에만 목적지로 할당받고 이동
                GameObject[] targets = GameObject.FindGameObjectsWithTag(robot.targetName);

                if(targets.Length > 0)
                {
                    target = targets[Random.Range(0, targets.Length)];
                    pathFinding.target = target;

                    robotState = RobotState.Move;
                    pathFinding.walkable = true;
                }
                else
                {
                    robotState = RobotState.Idel;
                    pathFinding.walkable = false;
                }
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
                    // 쓰레기 내려 놓는 행동 실행
                    isPickUp = false;
                    currentTime = 0;
                    robotState = RobotState.Idel;
                }
            }
            else if (robot.robotType == RobotType.Sweeper)
            {
                if (currentTime >= workTime)
                {
                    currentTime = 0;
                    robotState = RobotState.breaking;
                    Destroy(target);
                }
            }
        }
        #endregion

        #region 휴식 실행
        public void Break()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= breakTime)
            {
                currentTime = 0;
                robotState = RobotState.Idel;
            }
        }
        #endregion

        #region 쓰레기 놓기
        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // robot이 목적지에 도착하면 이동을 멈춤
            if (collision.collider.CompareTag(robot.targetName))
            {
                robotState = RobotState.Work;
                pathFinding.walkable = false;
            }
            else if (collision.collider.CompareTag("Destination") && robot.robotType == RobotType.Collector)
            {
                isPickUp = true;
                robotState = RobotState.Idel;
                pathFinding.walkable = false;
            }
        }
    }
}
