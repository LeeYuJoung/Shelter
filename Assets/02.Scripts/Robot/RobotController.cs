using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using UnityEngine.InputSystem;

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

        public RobotState robotState;
        public float moveSpeed;
        public float workTime;
        public float breakTime;
        public float range;

        public float currentTime;
        public bool isPickUp = false;  // ���� �κ��� ���

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
                    Move();
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
            robotAnimator = GetComponent<Animator>();
            pathFinding = GetComponent<PathFinding>();

            robotState = RobotState.Idel;
            moveSpeed = robot.moveSpeed;
            workTime = robot.workTime;
            breakTime = robot.breakTime;
            range = robot.range;

            currentTime = 0;
        }

        #region ������ ����
        public void TargetSetting()
        {
            if (robot.robotType == RobotType.Collector)
            {
                // ���� �κ��̶�� ������ �ܺ� �������� ������ �Ұ����� ������ ���� �������� �Ҵ�ް� �̵�
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

                    robotState = RobotState.Move;
                    pathFinding.walkable = true;
                }
            }
            else if(robot.robotType == RobotType.Sweeper)
            {
                // û�� �κ��̶�� �� ���� ������ ���� ��쿡�� �������� �Ҵ�ް� �̵�
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

        #region �̵� ����
        public void Move()
        {
            // �����¿� �̵��� ���� �ִϸ��̼� ����
            Vector2 dir = (target.transform.position - transform.position).normalized;

            if (dir.y > 0)
            {
                // �� �̵�

            }
            else if (dir.y < 0)
            {
                // �Ʒ� �̵�

            }
            else if (dir.x > 0)
            {
                // ������ �̵�

            }
            else if (dir.x < 0)
            {
                // ���� �̵�

            }
        }
        #endregion

        #region �۾� ����
        public void Work()
        {
            currentTime += Time.deltaTime;

            if (robot.robotType == RobotType.Collector)
            {
                if (currentTime >= workTime)
                {
                    currentTime = 0;
                    robotState = RobotState.Idel;

                    // ������ ��� �ִ� ��� ����

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

        #region û�� �κ� �޽� ����
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

        #region ���� �κ� �������� ����
        public IEnumerator Drop()
        {
            // ������ ���� ���� �ִϸ��̼� ����

            robotState = RobotState.Drop;
            pathFinding.walkable = false;

            yield return new WaitForSeconds(1.5f);

            isPickUp = false;
            robotState = RobotState.Idel;
        }
        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Destination") && robot.robotType == RobotType.Collector)
            {
                isPickUp = true;
                robotState = RobotState.Work;
                pathFinding.walkable = false;
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