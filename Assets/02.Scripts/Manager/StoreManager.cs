using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        private int collectorRobotPrice = 10;
        private int sweeperRobotPrice = 10;

        private int collectorRobotUpgradePrice = 15;
        private int sweeperRobotUpgradePrice = 15;

        const int collectorRobotMaxPiece = 3;
        const int sweeperRobotMaxPiece = 2;
        const int robotMaxUpgrade = 3;

        const int raderRoomUnLockPrice = 30;

        public int changeFuelAmount;
        public int changeGoldAmount;

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

        // 로봇 구매
        public void RobotBuy(string robotType)
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            if(robotType == "CollectorRobot")
            {
                if (GameManager.Instance.GetGold >= collectorRobotPrice && GameManager.Instance.collectorRobots.Count < collectorRobotMaxPiece)
                {
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(collectorRobotPrice);

                    collectorRobotPrice += 5;
                    UIManager.Instance.UpdateRobotBuyPriceText(1, collectorRobotPrice);

                    if (GameManager.Instance.collectorRobots.Count == collectorRobotMaxPiece)
                        UIManager.Instance.SoldOut(1, btn);
                }
                else
                {
                    // 구매 불가

                }
            }
            else if(robotType == "SweeperRobot")
            {
                if (GameManager.Instance.GetGold >= sweeperRobotPrice && GameManager.Instance.sweeperRobots.Count < sweeperRobotMaxPiece)
                {
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(sweeperRobotPrice);

                    sweeperRobotPrice += 5;
                    UIManager.Instance.UpdateRobotBuyPriceText(0, sweeperRobotPrice);

                    if (GameManager.Instance.sweeperRobots.Count == sweeperRobotMaxPiece)
                        UIManager.Instance.SoldOut(0, btn);
                }
                else
                {
                    // 구매 불가

                }
            }
        }

        // 로봇 업그레이드
        public void RobotUpgrade(string robotType)
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            if (robotType == "CollectorRobot")
            {
                if (GameManager.Instance.GetGold >= collectorRobotUpgradePrice && GameManager.Instance.collectorRobotLevel <= robotMaxUpgrade)
                {
                    UIManager.Instance.UpgradeState(1);
                    GameManager.Instance.collectorRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(collectorRobotUpgradePrice);

                    collectorRobotUpgradePrice += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(1, collectorRobotUpgradePrice);

                    if (GameManager.Instance.collectorRobotLevel > robotMaxUpgrade)
                        UIManager.Instance.UpgradeClear(1, btn);               
                }
                else
                {
                    // 업그레이드 불가

                }
            }
            else if(robotType == "SweeperRobot")
            {
                if (GameManager.Instance.GetGold >= sweeperRobotUpgradePrice && GameManager.Instance.sweeperRobotLevel <= robotMaxUpgrade)
                {
                    UIManager.Instance.UpgradeState(0);
                    GameManager.Instance.sweeperRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(sweeperRobotUpgradePrice);

                    sweeperRobotUpgradePrice += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(0, sweeperRobotUpgradePrice);

                    if (GameManager.Instance.sweeperRobotLevel > robotMaxUpgrade)
                        UIManager.Instance.UpgradeClear(0, btn);
                }
                else
                {
                    // 업그레이드 불가

                }
            }
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
            }
            else
            {
                // 해금 불가

            }
        }

        // 랜덤 tip 
        public void RandomTip()
        {

        }
    }
}