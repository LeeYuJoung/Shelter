using UnityEngine;
using EnumTypes;

namespace yjlee
{
    [CreateAssetMenu(fileName = "Robot", menuName = "Scriptable Objects/Robot")]
    public class Robot : ScriptableObject
    {
        public RobotType robotType;  // �κ� ����
        public string targetName;    // ���� ���
        public float moveSpeed;      // �̵� �ӵ�
        public float workTime;       // �۾� �ð�
        public float breakTime;      // ���� �ð�
        public float range;          // ��ֹ� �Ǵܽ� ����� �� ����
    }
}