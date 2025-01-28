using EnumTypes;
using System.Runtime.CompilerServices;
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

        [SerializeField] private int collectionRobotPrice = 200;
        [SerializeField] private int sweeperRobotPrice = 200;

        [SerializeField] private int collectionRobotUpgradePrice = 200;
        [SerializeField] private int sweeperRobotUpgradePrice = 200;

        private int robotMaxPiece = 3;

        // 로봇 구매
        public void RobotBuy(string robotType)
        {
            if(robotType == "CollectorRobot")
            {
                if(GameManager.Instance.GetcollectionRoboyPiece < robotMaxPiece && GameManager.Instance.GetGold >= collectionRobotUpgradePrice)
                {
                    collectionRobotUpgradePrice += 100;
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(collectionRobotUpgradePrice);
                    UIManager.Instance.UpdateRobotPriceText(0, collectionRobotUpgradePrice);
                }
                else
                {
                    Debug.Log("::: 구매 실패 :::");
                }
            }
            else
            {
                if (GameManager.Instance.GetsweeperRobotPiece < robotMaxPiece && GameManager.Instance.GetGold >= sweeperRobotUpgradePrice)
                {
                    sweeperRobotUpgradePrice += 100;
                    GameManager.Instance.RobotPiece(robotType);
                    GameManager.Instance.UseGold(sweeperRobotUpgradePrice);
                    UIManager.Instance.UpdateRobotPriceText(1, sweeperRobotUpgradePrice);
                }
                else
                {
                    Debug.Log("::: 구매 실패 :::");
                }
            }
        }

        // 로봇 업그레이드
        public void RobotUpgrade(string robotType)
        {

        }

        // 연료 판매
        public void FuelSale()
        {

        }
    }
}