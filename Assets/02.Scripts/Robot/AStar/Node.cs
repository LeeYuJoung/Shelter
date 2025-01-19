using UnityEngine;

namespace yjlee.Robot
{
    // Grid �ȿ� �� Node
    public class Node
    {
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;

        public Node parent;  // �� ������ ���� parent ����
        public int gCost;    // ������κ��� ���� �������� �ִܰŸ�
        public int hCost;    // ������������ ����Ÿ�

        public bool isWalkable;

        // gCost�� hCost�� ��ģ �������� ��ȯ
        public int fCost
        {
            get { return gCost + hCost; }
        }

        public Node(bool _walkable, Vector2 _WorldPos, int _x, int _y)
        {
            isWalkable = _walkable;     // ������ �� �ִ� �������
            worldPosition = _WorldPos;  // ����� ���� �� ��ġ��
            gridX = _x;                 // ����� x��ǥ ��
            gridY = _y;                 // ����� y��ǥ ��
        }
    }
}
