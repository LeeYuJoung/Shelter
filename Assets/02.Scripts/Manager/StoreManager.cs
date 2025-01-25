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
        public void RobotBuyAndUpgradeChange()
        {
            if (buyPhanel.activeSelf)
            {
                buyPhanel.SetActive(false);
                upgradePhanel.SetActive(true);
            }
            else
            {
                buyPhanel.SetActive(true);
                upgradePhanel.SetActive(false);
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