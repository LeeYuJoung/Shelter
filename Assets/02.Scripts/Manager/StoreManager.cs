using UnityEngine;

namespace yjlee.manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        public GameObject buyPhanel;
        public GameObject upgradePhanel;

        private int gold = 0;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(instance);
            }
        }

        // UIManager�� �̵��� ����
        public void RobotBuyAndUpgradeChange(int buttonDir)
        {
            if(buttonDir == 1)
            {

            }
            else
            {
                 
            }
        }

        // �κ� ����
        public void RobotBuy()
        {

        }

        // �κ� ���׷��̵�
        public void RobotUpgrade()
        {

        }

        // ���� �Ǹ�
        public void FuelSale()
        {

        }
    }
}