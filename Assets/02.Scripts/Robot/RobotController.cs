using System.Collections.Generic;
using UnityEngine;

namespace yjlee.robot
{
    public class RobotController : MonoBehaviour
    {
        private Animator robotAnimator;
        private PathFinding pathFinding;
        public Robot robot;

        public float moveSpeed;
        public float workSpeed;
        public float range;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {

        }

        private void Init()
        {
            robotAnimator = GetComponent<Animator>();
            pathFinding = GetComponent<PathFinding>();
            moveSpeed = robot.moveSpeed;
            workSpeed = robot.workSpeed;
            range = robot.range;
        }

        // 목적지 설정 함수
        public void TargetSetting()
        {
            // 만약, 수집 로봇이라면 랜덤한 외부 목적지와 쓰레기 소각장을 번갈아 가며 목적지로 할당받고 이동
            // 만약, 청소 로봇이라면 씬 내에 오물이 있을 경우 목적지로 할당받고 이동
        }

        // robot 타입에 따른 행동 함수 
        public void Action()
        {
            // 목적지에 도착했다면 행동 시작
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // robot이 목적지에 도착하면 이동을 멈춤
            if (collision.collider.CompareTag(robot.targetName))
            {
                pathFinding.walkable = false;
            }
        }
    }
}
