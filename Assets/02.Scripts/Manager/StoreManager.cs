using UnityEngine;

namespace Manager
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        private int collectorRobotPrice = 1000;
        private int sweeperRobotPrice = 1000;

        private int collectorRobotUpgradePrice = 1000;
        private int sweeperRobotUpgradePrice = 1000;

        private int robotMaxPiece = 3;

        public int changeFuelAmount;
        public int changeGoldAmount;

        // 로봇 구매
        public void RobotBuy(string robotType)
        {
            if(robotType == "CollectorRobot")
            {
                if (GameManager.Instance.collectorRobots.Count < robotMaxPiece)
                {
                    if (GameManager.Instance.GetGold >= collectorRobotPrice)
                    {
                        GameManager.Instance.RobotPiece(robotType);
                        GameManager.Instance.UseGold(collectorRobotPrice);

                        collectorRobotPrice += 1000;
                        UIManager.Instance.UpdateRobotBuyPriceText(0, collectorRobotPrice);
                    }
                    else
                    {
                        UIManager.Instance.Error("보유한 골드의 수량이 부족하여 구매 실패하였습니다.");
                    }
                }
                else
                {
                    UIManager.Instance.Error("보유 할 수 있는 수집 로봇의 수량을 넘었습니다.");
                }
            }
            else if(robotType == "sweeperRobot")
            {
                if (GameManager.Instance.sweeperRobots.Count < robotMaxPiece)
                {
                    if (GameManager.Instance.GetGold >= sweeperRobotPrice)
                    {
                        GameManager.Instance.RobotPiece(robotType);
                        GameManager.Instance.UseGold(sweeperRobotPrice);

                        sweeperRobotPrice += 1000;
                        UIManager.Instance.UpdateRobotBuyPriceText(1, sweeperRobotPrice);
                    }
                    else
                    {
                        UIManager.Instance.Error("보유한 골드의 수량이 부족하여 구매 실패하였습니다.");
                    }
                }
                else
                {
                    UIManager.Instance.Error("보유 할 수 있는 수집 로봇의 수량을 넘었습니다.");
                }
            }
        }

        // 로봇 업그레이드
        public void RobotUpgrade(string robotType)
        {
            if (robotType == "CollectorRobot")
            {
                if (GameManager.Instance.GetGold >= collectorRobotUpgradePrice)
                {
                    GameManager.Instance.RobotStatus(robotType);
                    GameManager.Instance.UseGold(collectorRobotUpgradePrice);

                    collectorRobotUpgradePrice += 1500;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(0, collectorRobotUpgradePrice);
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 업그레이드 실패하였습니다.");
                }
            }
            else if(robotType == "sweeperRobot")
            {
                if (GameManager.Instance.GetGold >= sweeperRobotUpgradePrice)
                {
                    GameManager.Instance.RobotStatus(robotType);
                    GameManager.Instance.UseGold(sweeperRobotUpgradePrice);
                    
                    sweeperRobotUpgradePrice += 1500;
                    UIManager.Instance.UpdateRoboUpgradetPriceText(1, sweeperRobotUpgradePrice);
                }
                else
                {
                    UIManager.Instance.Error("보유한 골드의 수량이 부족하여 업그레이드 실패하였습니다.");
                }
            }
        }

        // 연료 판매
        public void FuelSale()
        {
            GameManager.Instance.fuel -= changeFuelAmount;
            GameManager.Instance.GainGold(changeGoldAmount);
            UIManager.Instance.FuelSaleEnd();
        }
    }
}