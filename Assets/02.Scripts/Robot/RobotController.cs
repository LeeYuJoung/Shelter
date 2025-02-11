using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using UnityEngine.AI;
using Unity.VisualScripting;

namespace yjlee.robot
{
    public class Robotcontroller : MonoBehaviour
    {
        private Rigidbody2D robotRigidbody;
        private Animator robotAnimator;
        private PathFinding pathFinding;

        public Robot robot;
        public GameObject target;

        public GameObject[] trashPrefabs;
        private GameObject trash;

        [SerializeField] public RobotState robotState;
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
            robotAnimator = GetComponentInChildren<Animator>();
            pathFinding = GetComponent<PathFinding>();

            robotState = RobotState.Idel;
            moveSpeed = robot.moveSpeed;
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
        public void TargetSetting()
        {
            robotAnimator.SetFloat("MoveX", 0);
            robotAnimator.SetFloat("MoveY", 0);

            if (robot.robotType == RobotType.Collector)
            {
                // 수집 로봇이라면 랜덤한 외부 목적지와 쓰레기 소각장을 번갈아 가며 목적지로 할당받고 이동
                if (!isPickUp)
                {
                    GameObject[] targets = GameObject.FindGameObjectsWithTag("Destination");

                    if(targets.Length > 0)
                    {
                        target = targets[Random.Range(0, targets.Length)].gameObject;
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

        #region 이동 실행
        public void Move(bool isPickUp)
        {
            // 상하좌우 이동에 따라 애니메이션 조절
            Vector2 dir = (target.transform.position - transform.position).normalized;

            if (!isPickUp)
            {
                AnimatorSet("MoveX", "MoveY", dir);
            }
            else
            {
                AnimatorSet("PickUpX", "PickUpY", dir);
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
                    robotState = RobotState.Idel;
                }
            }
            else if (robot.robotType == RobotType.Sweeper)
            {
                if (currentTime >= workTime)
                {
                    currentTime = 0;
                    robotState = RobotState.Breaking;
                    Destroy(target);
                }
            }
        }
        #endregion

        #region 청소 로봇 휴식 실행
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

        #region 수집 로봇 내려놓기 실행
        public IEnumerator Drop()
        {
            // 쓰레기 내려 놓는 애니메이션 실행
            robotState = RobotState.Drop;
            pathFinding.walkable = false;
            robotAnimator.SetTrigger("Drop");

            yield return new WaitForSeconds(0.25f);

            isPickUp = false;
            robotState = RobotState.Idel;
            robotAnimator.SetBool("isPickUp", isPickUp);
        }
        #endregion

        // 애니메이션 조절
        public void AnimatorSet(string name1, string name2, Vector2 dir)
        {
            dir.x = (dir.x == 0) ? 0 : (dir.x > 0.7) ? 1 : (dir.x < -0.7) ? -1 : 0;
            dir.y = (dir.y == 0) ? 0 : (dir.y > 0.7) ? 1 : (dir.y < -0.7) ? -1 : 0;

            robotAnimator.SetFloat(name1, dir.x);
            robotAnimator.SetFloat(name2, dir.y);
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
                StartCoroutine(Drop());
            }
            else if (collision.collider.CompareTag(robot.targetName) && robot.robotType == RobotType.Sweeper)
            {
                robotState = RobotState.Work;
                pathFinding.walkable = false;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            robotRigidbody.linearVelocity = Vector3.zero;
            robotRigidbody.angularVelocity = 0.0f;
        }
    }
}