using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace yjlee.Robot
{
     /* A* Algorithm ����
     * OPEN SET : �򰡵Ǿ�� �� ��� ����
     * CLOSED SET : �̹� �򰡵� ��� ����
     * 
     * 1. OPEN SET���� ���� ���� fCost�� ���� ��� ȹ�� �� CLOSED SET ����
     * 2. �� ��尡 ��������� �ݺ��� Ż��
     * 3. �� ����� �ֺ� ������ CLOSED SET�� �ְ�, �ֺ� ����� f�� ��� (�ֺ� ����� g�� ���� �۴ٸ� f������ g�� �ֽ�ȭ)
     * 4. 1�� �ݺ�
     */
    public class PathFinding : MonoBehaviour
    {
        [Header("Path Finding")]
        public GameObject target;

        // Map�� ���ڷ� ����
        Grid grid;
        // �����Ÿ��� ���� Queue ����
        public Queue<Vector2> wayQueue = new Queue<Vector2>();

        // ��ȣ�ۿ� �� walkable�� false ���·� ��ȯ
        public static bool walkable = true;

        // �÷��̾� �̵�/ȸ�� �ӵ� ���� ������ ����
        public float moveSpeed;
        // ��ֹ� �Ǵܽ� ����� �� ����
        public float range;

        public bool isWalk;
        public bool isWalking;

        private void Awake()
        {
            // ���� ����
            grid = GameObject.Find("SpaceShip").GetComponent<Grid>();
            walkable = true;
        }

        private void Start()
        {
            // �ʱ�ȭ
            isWalking = false;
            moveSpeed = 20.0f;
            range = 4.0f;
        }

        private void FixedUpdate()
        {
            StartFindPaath((Vector2)transform.position, (Vector2)target.transform.position);
        }

        // Start to target �̵�
        public void StartFindPaath(Vector2 startPos, Vector2 targetPos)
        {
            StopAllCoroutines();
            StartCoroutine(FindPath(startPos, targetPos));
        }

        // ��ã�� ����
        IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
        {
            // Start, target�� ��ǥ�� grid�� ������ ��ǥ�� ����
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            bool pathSuccess = false;

            if (!targetNode.isWalkable)
                Debug.Log(":: UnWalkable StartNode ::");

            // walkable�� targetNode�� ��� ��ã�� ����
            if (targetNode.isWalkable)
            {
                // openSet, closedSet ����
                // closedSet�� �̹� ��� ����� ����
                // openSet�� ����� ��ġ�� �ִ� ����
                List<Node> openSet = new List<Node>();
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                // closedSet���� ���� ������ f�� ������ ��带 ��
                while (openSet.Count > 0)
                {
                    // currentNode�� ��� �� openSet���� ���� ��
                    Node currentNode = openSet[0];

                    // ��� openSet�� ����, current���� f���� �۰ų�, h(�޸���ƽ)���� ������ �װ��� current�� ����
                    for (int i = 1; i < openSet.Count; i++)
                    {
                        if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        {
                            currentNode = openSet[i];
                        }
                    }

                    // openSet���� current�� �� ��, closed�� �߰�
                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    // ��� ���� ��尡 �������� ���
                    if (currentNode == targetNode)
                    {
                        // seeker�� ��ġ�� ������ target�� �ƴ� ���
                        if (!pathSuccess)
                        {
                            PushWay(RetracePath(startNode, targetNode));
                        }

                        pathSuccess = true;
                        break;
                    }

                    // current�� �����¿� ���鿡 ���� g, hCost�� ���
                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                            continue;

                        // fCost ����
                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        // �̿����� ���� fCost�� �̿��� g���� ª�ų�, �湮�غ� openSet�� �� ���� ���ٸ�
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                        }
                    }
                }
            }

            yield return null;

            // ���� ã���� ���(����� �� ���� ���) �̵�
            if (pathSuccess)
            {
                // �̵��Ϸ��� ���� On
                isWalking = true;

                // wayQueue�� ���� �̵�
                while (wayQueue.Count > 0)
                {
                    var dir = wayQueue.First() - (Vector2)transform.position;
                    gameObject.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * moveSpeed * 5 * Time.deltaTime;

                    if ((Vector2)transform.position == wayQueue.First())
                    {
                        Debug.Log("Dequeue");
                        wayQueue.Dequeue();
                    }

                    yield return new WaitForSeconds(0.02f);
                }

                // �̵� ���̶�� ���� Off
            }
        }

        // WayQueue�� ���ο� Path�� �־��ֱ�
        private void PushWay(Vector2[] array)
        {
            wayQueue.Clear();
            foreach (Vector2 item in array)
            {
                wayQueue.Enqueue(item);
            }
        }

        // ���� Queue�� �Ųٷ� ����Ǿ������Ƿ�, �������� wayQueue�� ��������
        private Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            grid.path = path;
            Vector2[] wayPointns = SimplifyPath(path);

            return wayPointns;
        }

        // Node���� Vector ������ ����
        private Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> wayPoints = new List<Vector2>();

            for (int i = 0; i < path.Count; i++)
            {
                wayPoints.Add(path[i].worldPosition);
            }

            return wayPoints.ToArray();
        }

        // custom gCost �Ǵ� �޸���ƽ ����ġ�� ����ϴ� �Լ�
        // �Ű������� ������ ���� ���� ����� ����
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            return (distX > distY) ? (14 * distY + 10 * (distX - distY)) : (14 * distX + 10 * (distY - distX));
        }
    }
}