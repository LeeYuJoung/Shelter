using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        public GameObject[] repairButtons;
        public GameObject[] upgradeButtons;
        public GameObject raderRoomButton;

        public Text tipText;

        private int[] robotPrices = new int[2] { 10, 10 };
        private int[] robotUpgradePrices = new int[2] { 15, 15 };

        private int[] robotMxPieces = new int[2] { 2, 3 };
        private int robotMaxUpgrade = 3;

        const int raderRoomUnLockPrice = 30;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        public void SetRepairButton()
        {
            for (int i = 0; i < repairButtons.Length; i++)
            {
                UIManager.Instance.RepairPossible(repairButtons[i], GameManager.Instance.GetGold >= robotPrices[i]);
            }

            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                UIManager.Instance.RepairPossible(upgradeButtons[i], GameManager.Instance.GetGold >= robotUpgradePrices[i]);
            }

            UIManager.Instance.RepairPossible(raderRoomButton, GameManager.Instance.GetGold >= raderRoomUnLockPrice);
        }

        // 로봇 구매
        public void RobotBuy(int robotType)
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            if (robotType == 1)
            {
                if (GameManager.Instance.GetGold >= robotPrices[1] && GameManager.Instance.collectorRobots.Count < robotMxPieces[1])
                {
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(robotPrices[1]);

                    robotPrices[1] += 5;
                    UIManager.Instance.UpdateRobotBuyPriceText(1, robotPrices[1]);

                    if (GameManager.Instance.collectorRobots.Count == robotMxPieces[1])
                        UIManager.Instance.SoldOut(1, btn);
                }
            }
            else if (robotType == 0)
            {
                if (GameManager.Instance.GetGold >= robotPrices[0] && GameManager.Instance.sweeperRobots.Count < robotMxPieces[0])
                {
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(robotPrices[0]);

                    robotPrices[0] += 5;
                    UIManager.Instance.UpdateRobotBuyPriceText(0, robotPrices[0]);

                    if (GameManager.Instance.sweeperRobots.Count == robotMxPieces[0])
                        UIManager.Instance.SoldOut(0, btn);
                }
            }

            SetRepairButton();
        }

        // 로봇 업그레이드
        public void RobotUpgrade(int robotType)
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            if (robotType == 1)
            {
                if (GameManager.Instance.GetGold >= robotUpgradePrices[1] && GameManager.Instance.collectorRobotLevel <= robotMaxUpgrade)
                {
                    UIManager.Instance.UpgradeState(1);
                    GameManager.Instance.collectorRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(robotUpgradePrices[1]);

                    robotUpgradePrices[1] += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(1, robotUpgradePrices[1]);

                    if (GameManager.Instance.collectorRobotLevel > robotMaxUpgrade)
                        UIManager.Instance.UpgradeClear(1, btn);               
                }
            }
            else if(robotType == 0)
            {
                if (GameManager.Instance.GetGold >= robotUpgradePrices[0] && GameManager.Instance.sweeperRobotLevel <= robotMaxUpgrade)
                {
                    UIManager.Instance.UpgradeState(0);
                    GameManager.Instance.sweeperRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(robotUpgradePrices[0]);

                    robotUpgradePrices[0] += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(0, robotUpgradePrices[0]);

                    if (GameManager.Instance.sweeperRobotLevel > robotMaxUpgrade)
                        UIManager.Instance.UpgradeClear(0, btn);
                }
            }

            SetRepairButton();
        }

        // 레이더실 해금
        public void RaderRoomUnLock()
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            if (GameManager.Instance.GetGold >= raderRoomUnLockPrice)
            {
                UIManager.Instance.RaderRoomUnLock(btn);
                GameManager.Instance.isRadeRoomUnLock = true;
                GameManager.Instance.UseGold(raderRoomUnLockPrice);
                MiniGameManager.Instance.possibleIndex = 3;
                SetRepairButton();
            }
        }

        // 랜덤 tip 
        public void RandomTip()
        {

        }
    }
}