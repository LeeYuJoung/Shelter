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

        // UIManager로 이동할 예정
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

        // 로봇 구매
        public void RobotBuy()
        {

        }

        // 로봇 업그레이드
        public void RobotUpgrade()
        {

        }

        // 연료 판매
        public void FuelSale()
        {

        }
    }
}