using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        public Status status;

        public GameObject[] buyPhanels;
        public GameObject errorPhanel;

        public Text[] goldTexts;
        public Text[] robotBuyPriceTexts;
        public Text[] robotUpgradePriceTexts;
        public Text robotPieceText;
        public Text fuelText;
        public Text goldOfFuel;

        public InputField saleInput;
        public Sprite soldOutImage;

        private int buyPhanelIndex;

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
            buyPhanelIndex = 0;
            status = GameObject.Find("Status").GetComponent<Status>();
            fuelText.text = string.Format("{0} L", status.statusData.FuelAmount);
        }

        // 디데이 변경
        public void UpdateDayImage(int day)
        {

        }

        // 하루 시간 변경
        public void UpdateTimeImage()
        {

        }

        // 골드 텍스트 변경
        public void UpdateGoldText(int gold)
        {
            for(int i = 0; i < goldTexts.Length; i++)
            {
                goldTexts[i].text = string.Format("{0:N0} G", gold);
            }
        }

        // 로봇 구매 가격 텍스트 변경
        public void UpdateRobotBuyPriceText(int index, int price)
        {
            robotBuyPriceTexts[index].text = string.Format("{0:N0} G", price);
        }

        // 로봇 업그레이드 가격 텍스트 변경
        public void UpdateRoboUpgradetPriceText(int index, int price)
        {
            robotUpgradePriceTexts[index].text = string.Format("{0:N0} G", price);
        }

        // 로봇 수량 텍스트 변경
        public void UpdateRobotPieceText(int collectorRobotPiece, int sweeperRobotPiece)
        {
            robotPieceText.text = string.Format("수집로봇 X {0}   청소로봇 X {1}", collectorRobotPiece, sweeperRobotPiece);
        }

        public void SoldOut(GameObject btn)
        {
            Button button = btn.GetComponent<Button>();
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(319, 193);
            button.image.sprite = soldOutImage;
            button.interactable = false;
        }

        // 상점 구매창 초기화
        public void BuyPhanelInit()
        {
            buyPhanelIndex = 0;

            for (int i = 0; i < buyPhanels.Length; i++)
            {
                if(i == 0)
                    buyPhanels[i].SetActive(true);
                else
                    buyPhanels[i].SetActive(false);
            }
        }

        // 상점 구매창 변경
        public void ChangeBuyPhanel(int arrowDir)
        {
            buyPhanels[buyPhanelIndex].SetActive(false);

            if (buyPhanelIndex >= buyPhanels.Length - 1 && arrowDir > 0)
            {
                buyPhanelIndex = 0;
            }
            else if(buyPhanelIndex == 0 && arrowDir < 0)
            {
                buyPhanelIndex = 2;
            }
            else
            {
                buyPhanelIndex += arrowDir;
            }

            buyPhanels[buyPhanelIndex].SetActive(true);
        }

        // 플레이어의 연료 판매량 설정
        public void ChangeFuelAmount()
        {
            try
            {
                StoreManager.Instance.changeFuelAmount = int.Parse(saleInput.text.ToString().Replace("L", ""));
                StoreManager.Instance.changeGoldAmount = StoreManager.Instance.changeFuelAmount * 150;
                saleInput.text = string.Format("{0} L", StoreManager.Instance.changeFuelAmount);
                goldOfFuel.text = string.Format("{0:N0} G", StoreManager.Instance.changeGoldAmount);

                if (status.statusData.FuelAmount < StoreManager.Instance.changeFuelAmount)
                {
                    Error("보유한 연료량이 적어 판매 불가능합니다.");
                    FuelSaleEnd();
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
                StoreManager.Instance.changeFuelAmount = 0;
                StoreManager.Instance.changeGoldAmount = 0;
                goldOfFuel.text = string.Format("{0:N0} G", 0);
            }
        }

        // 플레이어의 연료 판매 완료
        public void FuelSaleEnd()
        {
            saleInput.text = null;
            goldOfFuel.text = string.Format("{0:N0} G", 0);
            fuelText.text = string.Format("{0} L", status.statusData.FuelAmount);
            StoreManager.Instance.changeFuelAmount = 0;
            StoreManager.Instance.changeGoldAmount = 0;
        }

        public void Error(string message)
        {
            errorPhanel.GetComponentInChildren<Text>().text = message;
            StartCoroutine(OnError());
        }

        public IEnumerator OnError()
        {
            errorPhanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            errorPhanel.SetActive(false);
        }
    }
}
