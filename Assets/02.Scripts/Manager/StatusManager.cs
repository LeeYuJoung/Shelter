using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class StatusManager : MonoBehaviour
    {
        private static StatusManager instance;
        public static StatusManager Instance { get { return instance; } }

        public Status status;

        public Text partRepairPriceText;
        public Text repairGaugeText;
        public Image partImage;
        public Image repairGauge;
        public Image corrosionGauge;
        public Image fuelAmountGauge;
        public Image radarOutputAmountGauge;
        public Sprite[] repairSprites;
        public Sprite[] repairClearSprites;

        public GameObject repairButton;

        public GameObject radarOutputAmountGameObject;
        public Image hullRestorationRateImage;
        public Image moterImage;
        public Image engineImage;
        public Image radarImage;

        public Sprite[] repairGaugeSprites;            // 수리 게이지 이미지
        public Sprite[] fuelAmountGaugeSprites;        // 연료량 게이지 이미지
        public Sprite[] radarOutputAmountGaugeSprites; // 레이더 출력량 게이지 이미지

        public Sprite[] partSprites;   // 부품 이미지
        public int[] partPrices;      // 부품 수리 비용
        public bool[] isRepairClear;  // 부품 수리 완료 상태

        private int currentPartIndex = 0;
        private float currentTime = 0;
        private float radarOutputAmountGaugeUpTime = 35.0f;

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

        private void Update()
        {
            // 레이더 출력량 증가
            if (status.statusData.RadarRestorationRate >= 40 && status.statusData.RadarOutputAmount < 100)
            {
                currentTime += Time.deltaTime;
                radarOutputAmountGameObject.SetActive(true);

                if (currentTime >= radarOutputAmountGaugeUpTime)
                {
                    currentTime = 0;
                    RadarOutputAmountGaugeChange(true);
                }
            }
        }

        // 초기화
        private void Init()
        {
            partPrices = new int[] { 5, 5, 5, 5};
            isRepairClear = new bool[] { false, false, false, false};

            status = GetComponent<Status>();
        }

        // 수리할 부품 선택
        public void PartSelect(int partIndex)
        {
            currentPartIndex = partIndex;
            StatusGaugeControl(partIndex);
            StatusPhanelSetting(partSprites[partIndex], partPrices[partIndex]);
        }

        // Status UI 세팅
        public void StatusPhanelSetting(Sprite partSprite, int partPrice)
        {
            partImage.sprite = partSprite;
            partRepairPriceText.text = string.Format("x {0}", partPrice.ToString());
        }

        // Status 게이지 조절
        public void StatusGaugeControl(int partIndex)
        {
            if (partIndex == 3 && !GameManager.Instance.isRadeRoomUnLock)
                UIManager.Instance.RepairPossible(repairButton, false);
            else
                UIManager.Instance.RepairPossible(repairButton, (GameManager.Instance.GetGold >= partPrices[partIndex] && !isRepairClear[partIndex]));

            switch (partIndex)
            {
                case 0:
                    repairGaugeText.text = string.Format("{0}%", status.statusData.HullRestorationRate);
                    repairGauge.sprite = repairGaugeSprites[Mathf.FloorToInt(status.statusData.HullRestorationRate / 10)];
                    break;
                case 1:
                    repairGaugeText.text = string.Format("{0}%", status.statusData.MotorRestorationRate);
                    repairGauge.sprite = repairGaugeSprites[Mathf.FloorToInt(status.statusData.MotorRestorationRate / 10)];
                    break;
                case 2:
                    repairGaugeText.text = string.Format("{0}%", status.statusData.EngineRestorationRate);
                    repairGauge.sprite = repairGaugeSprites[Mathf.FloorToInt(status.statusData.EngineRestorationRate / 10)];
                    break;
                case 3:
                    repairGaugeText.text = string.Format("{0}%", status.statusData.RadarRestorationRate);
                    repairGauge.sprite = repairGaugeSprites[Mathf.FloorToInt(status.statusData.RadarRestorationRate / 10)];
                    break;
            }
        }

        // Status UI에서 수리 버튼 클릭 시 실행
        public void Reapir()
        {
            if(GameManager.Instance.GetGold >= partPrices[currentPartIndex] && !isRepairClear[currentPartIndex])
            {
                AudioManager.Instance.PlaySFX(4);
                GameManager.Instance.UseGold(partPrices[currentPartIndex]);
                partPrices[currentPartIndex] += 5;

                switch (currentPartIndex)
                {
                    case 0:
                        status.SetHullRestorationRate(true);
                        PartSelect(currentPartIndex);

                        if (status.statusData.HullRestorationRate >= 100)
                            RepairClear(0, hullRestorationRateImage, true);

                        break;
                    case 1:
                        status.SetMotorRestorationRate(true);
                        PartSelect(currentPartIndex);

                        if (status.statusData.MotorRestorationRate >= 100)
                            RepairClear(1, moterImage, true);

                        break;
                    case 2:
                        status.SetEngineRestorationRate(true);
                        PartSelect(currentPartIndex);

                        if (status.statusData.EngineRestorationRate >= 100)
                            RepairClear(2, engineImage, true);

                        break;
                    case 3:
                        status.SetRadarRestorationRate(true);
                        PartSelect(currentPartIndex);
                        radarOutputAmountGaugeUpTime -= 2.0f;

                        if (status.statusData.RadarRestorationRate >= 100)
                            RepairClear(3, radarImage, true);

                        break;
                }
            }
        }

        // 연료 스테이터스 변경
        public void FuelGaugeChange()
        {
            if (status.statusData.FuelAmount + 5 > 100)
                return;

            status.SetFuelAmount(true);
            fuelAmountGauge.sprite = fuelAmountGaugeSprites[Mathf.FloorToInt(status.statusData.FuelAmount / 10)];
        }

        // 레이더출력량 스테이터스 변경
        public void RadarOutputAmountGaugeChange(bool isUp)
        {
            if (isUp && status.statusData.RadarOutputAmount + 5 > 100)
                return;
            if (!isUp && status.statusData.RadarOutputAmount - 5 < 0)
                return;

            Debug.Log(":: Status up ::");
            status.SetRadarOutputAmount(isUp);
            radarOutputAmountGauge.sprite = radarOutputAmountGaugeSprites[Mathf.FloorToInt(status.statusData.RadarOutputAmount / 10)];
        }

        // 수리 완료
        public void RepairClear(int partIndex, Image partImage, bool isClear)
        {
            isRepairClear[partIndex] = isClear;

            if(isClear)
                partImage.sprite = repairClearSprites[partIndex];
            else
                partImage.sprite = repairSprites[partIndex];
        }
    }
}
