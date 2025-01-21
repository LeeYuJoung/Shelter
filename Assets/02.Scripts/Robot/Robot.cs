using UnityEngine;
using EnumTypes;

namespace yjlee
{
    [CreateAssetMenu(fileName = "Robot", menuName = "Scriptable Objects/Robot")]
    public class Robot : ScriptableObject
    {
        public RobotType robotType;  // 로봇 종류
        public string targetName;    // 추적 대상
        public float moveSpeed;      // 이동 속도
        public float workSpeed;      // 작업 속도
        public float range;          // 장애물 판단시 멈출게 할 범위
    }
}