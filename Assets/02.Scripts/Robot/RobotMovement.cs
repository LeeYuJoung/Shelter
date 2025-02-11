using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

namespace yjlee.robot
{
    public class RobotMovement : MonoBehaviour
    {
        private Rigidbody2D robotRigidbody;
        private Animator robotAnimator;
        private NavMeshAgent agent;

        public Robot robot;
        public GameObject target;

        [SerializeField] public RobotState robotState;
        public float moveSpeed;
        public float workTime;
        public float breakTime;
        public float range;

        private void Start()
        {
            Init();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        private void Update()
        {
            
        }

        private void Init()
        {
            robotRigidbody = GetComponent<Rigidbody2D>();
            robotAnimator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }
    }
}
