using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        public GameObject buyPhanel;
        public GameObject upgradePhanel;

        public Text dayText;
        public Text[] goldTexts;
        public Text[] robotPriceText;

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

        public void UpdateDayText(int day)
        {
            dayText.text = string.Format("{0:D3}", day);
        }

        public void UpdateGoldText(int gold)
        {
            for(int i = 0; i < goldTexts.Length; i++)
            {
                goldTexts[i].text = string.Format("{0:N0}G", gold);
            }
        }

        public void UpdateRobotPriceText(int index, int price)
        {
            robotPriceText[index].text = string.Format("{0:N0}G", price);
        }

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
    }
}
