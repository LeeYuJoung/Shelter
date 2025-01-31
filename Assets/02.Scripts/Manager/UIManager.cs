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

        public Text dayText;
        public Text[] goldTexts;
        public Text[] robotBuyPriceText;
        public Text[] robotUpgradePriceText;
        public Text fuelText;
        public Text goldOfFuel;

        public InputField saleInput;
        public Sprite soldOutImage;

        private int buyPhanelIndex = 0;

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

        // 디데이 텍스트 변경
        public void UpdateDayText(int day)
        {
            dayText.text = string.Format("{0:D3}", day);
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
            robotBuyPriceText[index].text = string.Format("{0:N0} G", price);
        }

        // 로봇 업그레이드 가격 텍스트 변경
        public void UpdateRoboUpgradetPriceText(int index, int price)
        {
            robotUpgradePriceText[index].text = string.Format("{0:N0} G", price);
        }

        public void SoldOut(GameObject btn)
        {
            Button button = btn.GetComponent<Button>();
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(213, 129);
            button.image.sprite = soldOutImage;
            button.interactable = false;
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

                if (StoreManager.Instance.changeFuelAmount > 100)
                {
                    Error("연료는 100이하로만 판매 가능합니다.");
                    FuelSaleEnd();
                }
                else if (GameManager.Instance.fuel < StoreManager.Instance.changeFuelAmount)
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
            fuelText.text = string.Format("{0} L", GameManager.Instance.fuel);
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
