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

        public GameObject buyPhanel;
        public GameObject upgradePhanel;
        public GameObject errorPhanel;

        public Text dayText;
        public Text[] goldTexts;
        public Text[] robotBuyPriceText;
        public Text[] robotUpgradePriceText;
        public Text fuelText;
        public Text goldOfFuel;

        public InputField saleInput;

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

        // 로봇 구매창과 업그레이드창 변환
        public void ChangeBuyAndUpgrade()
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

        // 플레이어의 연료 판매량 설정
        public void ChangeFuelAmount()
        {
            try
            {
                StoreManager.Instance.changeFuelAmount = int.Parse(saleInput.text);
                StoreManager.Instance.changeGoldAmount = StoreManager.Instance.changeFuelAmount * 150;
                goldOfFuel.text = string.Format("{0:N0} G", StoreManager.Instance.changeGoldAmount);

                if(StoreManager.Instance.changeFuelAmount >= 100)
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
            }
        }

        // 플레이어가 연료 판매 완료
        public void FuelSaleEnd()
        {
            saleInput.text = null;
            goldOfFuel.text = string.Format("{0} G", 0);
            fuelText.text = string.Format("{0:D3}", GameManager.Instance.fuel);
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
