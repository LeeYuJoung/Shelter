using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        private int collectorRobotPrice = 1000;
        private int sweeperRobotPrice = 1000;

        private int collectorRobotUpgradePrice = 10;
        private int sweeperRobotUpgradePrice = 10;

        const int collectorRobotMaxPiece = 3;
        const int sweeperRobotMaxPiece = 2;
        const int robotMaxUpgrade = 3;

        const int raderRoomUnLockPrice = 20;

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

            Init();
        }

        private void Init()
        {

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
                    UIManager.Instance.UpdateRobotBuyPriceText(0, collectorRobotPrice);

                    if (GameManager.Instance.collectorRobots.Count == collectorRobotMaxPiece)
                        UIManager.Instance.SoldOut(btn);
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 구매 실패하였습니다.");
                }
            }
            else if(robotType == "SweeperRobot")
            {
                if (GameManager.Instance.GetGold >= sweeperRobotPrice && GameManager.Instance.sweeperRobots.Count < sweeperRobotMaxPiece)
                {
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(sweeperRobotPrice);

                    sweeperRobotPrice += 5;
                    UIManager.Instance.UpdateRobotBuyPriceText(1, sweeperRobotPrice);

                    if (GameManager.Instance.sweeperRobots.Count == sweeperRobotMaxPiece)
                        UIManager.Instance.SoldOut(btn);
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 구매 실패하였습니다.");
                }
            }
        }

        // 로봇 업그레이드
        public void RobotUpgrade(string robotType)
        {
            if (robotType == "CollectorRobot")
            {
                if (GameManager.Instance.GetGold >= collectorRobotUpgradePrice && GameManager.Instance.collectorRobotLevel < robotMaxUpgrade)
                {
                    GameManager.Instance.collectorRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(collectorRobotUpgradePrice);

                    collectorRobotUpgradePrice += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(0, collectorRobotUpgradePrice);

                    if (GameManager.Instance.collectorRobotLevel >= robotMaxUpgrade)
                        UIManager.Instance.Error("업그레이드가 완료되어 불가능합니다.");
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 업그레이드 실패하였습니다.");
                }
            }
            else if(robotType == "SweeperRobot")
            {
                if (GameManager.Instance.GetGold >= sweeperRobotUpgradePrice && GameManager.Instance.sweeperRobotLevel < robotMaxUpgrade)
                {
                    GameManager.Instance.sweeperRobotLevel += 1;
                    GameManager.Instance.RobotStatusUp(robotType);
                    GameManager.Instance.UseGold(sweeperRobotUpgradePrice);

                    sweeperRobotUpgradePrice += 5;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(1, sweeperRobotUpgradePrice);

                    if (GameManager.Instance.sweeperRobotLevel >= robotMaxUpgrade)
                        UIManager.Instance.Error("보유한 골드의 수량이 부족하여 업그레이드 실패하였습니다.");
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 업그레이드 실패하였습니다.");
                }
            }
        }

        // 레이더실 해금
        public void RaderRoomUnLock()
        {
            if(GameManager.Instance.GetGold >= raderRoomUnLockPrice)
            {
                GameManager.Instance.isRadeRoomUnLock = true;
                GameManager.Instance.UseGold(raderRoomUnLockPrice);
            }
            else
            {
                UIManager.Instance.Error("보유한 골드의 수량이 부족하여 레이더실 해금에 실패하였습니다.");
            }
        }

        // 연료 판매
        public void FuelSale()
        {
            StatusManager.Instance.status.statusData.FuelAmount -= changeFuelAmount;
            StatusManager.Instance.FuelGaugeChange();
            GameManager.Instance.GainGold(changeGoldAmount);
            UIManager.Instance.FuelSaleEnd();
        }
    }
}