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

        // ������ ���� �Լ�
        public void TargetSetting()
        {
            if(robot.robotType == RobotType.Collector)
            {
                // ����, ���� �κ��̶�� ������ �ܺ� �������� ������ �Ұ����� ������ ���� �������� �Ҵ�ް� �̵�

            }
            else
            {
                // ����, û�� �κ��̶�� �� ���� ������ ���� ��� �������� �Ҵ�ް� �̵�

            }
        }

        // robot Ÿ�Կ� ���� �ൿ �Լ� 
        public void Action()
        {
            // �������� �����ߴٸ� �ൿ ����
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // robot�� �������� �����ϸ� �̵��� ����
            if (collision.collider.CompareTag(robot.targetName))
            {
                pathFinding.walkable = false;
            }
        }
    }
}
