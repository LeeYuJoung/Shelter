using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        public GameObject raderRoomLock;
        public GameObject[] raderRoomSweeperPos;

        public GameObject[] repairButtons;
        public GameObject[] upgradeButtons;
        public GameObject raderRoomButton;

        private int[] robotPrices = new int[2] { 10, 10 };
        private int[] robotUpgradePrices = new int[2] { 15, 15 };

        private int[] robotMxPieces = new int[2] { 2, 3 };
        private int robotMaxUpgrade = 3;

        const int raderRoomUnLockPrice = 30;

        private bool[] isClear = new bool[5] { false, false, false, false, false };

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

        // 구매 or 불가 버튼 변경
        public void SetRepairButton()
        {
            // 구매 버튼
            for (int i = 0; i < repairButtons.Length; i++)
            {
                UIManager.Instance.RepairPossible(repairButtons[i], (GameManager.Instance.GetGold >= robotPrices[i]) && !isClear[i]);
            }
            // 업그레이드 버튼
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                UIManager.Instance.RepairPossible(upgradeButtons[i], (GameManager.Instance.GetGold >= robotUpgradePrices[i] && !isClear[2 + i]));
            }
            // 레이더실 해금 버튼
            UIManager.Instance.RepairPossible(raderRoomButton, (GameManager.Instance.GetGold >= raderRoomUnLockPrice && !isClear[4]));
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
                    {
                        isClear[1] = true;
                        UIManager.Instance.SoldOut(1, btn);
                    }
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
                    {
                        isClear[0] = true;
                        UIManager.Instance.SoldOut(0, btn);
                    }
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
                    {
                        isClear[3] = true;
                        UIManager.Instance.UpgradeClear(1, btn);
                    }
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
                    {
                        isClear[2] = true;
                        UIManager.Instance.UpgradeClear(0, btn);
                    }
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
                raderRoomLock.SetActive(false);
                UIManager.Instance.RaderRoomUnLock(btn);
                GameManager.Instance.isRadeRoomUnLock = true;
                GameManager.Instance.UseGold(raderRoomUnLockPrice);
                MiniGameManager.Instance.possibleIndex = 3;
                SetRepairButton();
                isClear[4] = true;

                for(int i = 0; i < raderRoomSweeperPos.Length; i++)
                {
                    raderRoomSweeperPos[i].SetActive(true);
                }
            }
        }
    }
}