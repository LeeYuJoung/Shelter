using UnityEngine;
using UnityEngine.AI;

namespace yjlee.robot
{
    public class RobotMovement : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        private void Update()
        {
            
        }
    }
}
