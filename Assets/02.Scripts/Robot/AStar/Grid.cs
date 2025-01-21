using System.Collections.Generic;
using UnityEngine;

namespace yjlee.robot
{    
    // ��ũ���� Grid - Node�� ����
    // ������ x, y �������� ������ ���� ��, ������ ������ŭ �����ȿ� ������ ����
    public class Grid : MonoBehaviour
    {
        public Transform robot;
        public LayerMask unwalkableMask; // ���� �� ���� ���̾� 
        public Vector2 gridSize;         // ȭ���� ũ��

        private Node[,] grid;
        public float nodeRadius;    // ������
        private float nodeDiameter; // ������ ���� 
        private int gridSizeX;
        private int gridSizeY;

        [SerializeField]
        public List<Node> path; // A*���� ����� Path  

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;  // ������ ���������� ���� ����
            gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);   // �׸����� ���� ũ��
            gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);   // �׸����� ���� ũ��

            CreateGrid();
        }

        #region ���� ���� �Լ�
        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];

            // ���� ������ ���� ���ϴܺ��� ���� (transform�� ���� �߾ӿ� ��ġ)
            // �̿� x�� y��ǥ�� �ݹ� �� ����, �Ʒ������� �ű�
            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridSize.x / 2 - Vector2.up * gridSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);

                    // �ش� ���ڰ� walkable���� �ƴ��� �Ǵ�
                    bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                    // ��� �Ҵ�
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }
        #endregion

        #region node ���� �¿� �밢 ��带 ��ȯ�ϴ� �Լ�
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        if (!grid[node.gridX, checkY].isWalkable && !grid[checkX, node.gridY].isWalkable)
                            continue;
                        if (!grid[node.gridX, checkY].isWalkable || !grid[checkX, node.gridY].isWalkable)
                            continue;

                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }
        #endregion

        #region �Է����� ���� ������ǥ�� node ��ǥ��� ��ȯ�ϴ� �Լ�
        public Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
            float percentY = (worldPosition.y + gridSize.y / 2) / gridSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return grid[x, y];
        }
        #endregion

        #region Scene View ��¿� Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(gridSize.x, gridSize.y));

            if (grid != null)
            {
                Node playerNode = NodeFromWorldPoint(robot.position);

                foreach (Node n in grid)
                {
                    Gizmos.color = (n.isWalkable) ? new Color(1, 1, 1, 0.3f) : new Color(1, 0, 0, 0.3f);

                    if (!n.isWalkable)
                    {
                        if (path != null)
                        {
                            if (path.Contains(n))
                            {
                                Gizmos.color = new Color(0, 0, 0, 0.3f);
                                Debug.Log("?");
                            }
                        }
                    }

                    if (playerNode == n)
                    {
                        Gizmos.color = new Color(0, 1, 1, 0.3f);
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
                }
            }
        }
        #endregion
    }
}