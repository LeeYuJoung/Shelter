using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class StatusManager : MonoBehaviour
    {
        private static StatusManager instance;
        public static StatusManager Instance { get { return instance; } }

        public Status status;

        public Text partNameText;
        public Image partImage;
        public Text partDescriptionText;
        public Text partRepairPriceText;
        public Slider partStatusGauge;
        public Button partRepairButton;   
        public Sprite[] partRepairButtonSprites;

        private int currentPartIndex;

        private string[] partNames;      // 부품 이름
        public Sprite[] partSprites;     // 부품 이미지
        private string[] descriptions;   // 부품 설명
        private int[] partPrices;        // 부품 수리 비용
        private bool[] isRepairClear;    // 부품 수리 완료 상태

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

        // 초기화
        private void Init()
        {
            status = GetComponent<Status>();

            partNames = new string[] { "오염도", "연료통", "모터", "엔진", "레이더", "레이더 출력량" };
            descriptions = new string[] {
                "오염도는 저어닐 밎ㄷㅂ잳 가나나 ㅁㄷㅇㅂㅈㄷ",
                "선체 복구도를 높이면 어쩌구 저저꺼주거 ㅇㅁㅇㅁㄴㅇ",
                "모터 복구도 ㅘ라란ㅇ리 넌 ㅇㅁㄴ오ㅑㅈㄷ",
                "엔진 복구도 블라라라 ㅇ너미쟈 ㅜㅁㅈㄷㅇ  ㅇㅁㅇ",
                "레이더 복구도 쏘라라라 ㅏ지ㅏ민ㅇ 어쩌구 저저ㅉ거 ㅁㅇ ㅁㄴㅇ",
                "레이더 출력랴으으ㅜㅁㄴ얌젿뱌더밎뎔기ㅓㅇㄹ ㅁㄴㅇㅁㄴㅇ  ㅁㄴㅇ "};
            partPrices = new int[] { 0, 1000, 1000, 1000, 1000, 0 };
            isRepairClear = new bool[] { false, false, false, false, false, false };
        }

        // 수리할 부품 선택
        public void PartSelect(int partIndex)
        {
            currentPartIndex = partIndex;
            StatusGaugeControl(partIndex);
            StatusPhanelSetting(partNames[partIndex], partSprites[partIndex], descriptions[partIndex], partPrices[partIndex]);
        }

        // Status UI 세팅
        public void StatusPhanelSetting(string partName, Sprite partSprite, string description, int partPrice)
        {
            partNameText.text = partName;
            partImage.sprite = partSprite;
            partDescriptionText.text = description;
            partRepairPriceText.text = partPrice.ToString();
        }

        // Status 게이지 조절
        public void StatusGaugeControl(int partIndex)
        {
            switch (partIndex)
            {
                case 0:
                    partStatusGauge.value = status.statusData.Corrosion / 100.0f;
                    RepairImpossible(false);
                    break;
                case 1:
                    partStatusGauge.value = status.statusData.HullRestorationRate / 100.0f;
                    RepairImpossible(true);
                    break;
                case 2:
                    partStatusGauge.value = status.statusData.MotorRestorationRate / 100.0f;
                    RepairImpossible(true);
                    break;
                case 3:
                    partStatusGauge.value = status.statusData.EngineRestorationRate / 100.0f;
                    RepairImpossible(true);
                    break;
                case 4:
                    partStatusGauge.value = status.statusData.RadarRestorationRate / 100.0f;
                    RepairImpossible(true);
                    break;
                case 5:
                    partStatusGauge.value = status.statusData.RadarOutputAmount / 100.0f;
                    RepairImpossible(false);
                    break;
            }
        }

        // Status UI에서 수리 버튼 클릭 시 실행
        public void Reapir()
        {
            if(GameManager.Instance.GetGold >= partPrices[currentPartIndex])
            {
                if (!isRepairClear[currentPartIndex])
                {
                    GameManager.Instance.UseGold(partPrices[currentPartIndex]);
                    partPrices[currentPartIndex] += 100;

                    switch (currentPartIndex)
                    {
                        case 1:
                            status.SetHullRestorationRate(true);
                            PartSelect(currentPartIndex);
                            break;
                        case 2:
                            status.SetMotorRestorationRate(true);
                            PartSelect(currentPartIndex);
                            break;
                        case 3:
                            status.SetEngineRestorationRate(true);
                            PartSelect(currentPartIndex);
                            break;
                        case 4:
                            status.SetRadarRestorationRate(true);
                            PartSelect(currentPartIndex);
                            break;
                    }
                }
                else
                {
                    // 수리 완료

                }
            }
            else
            {
                // 구매 불가

            }
        }

        // 수리 가능 여부 확인
        public void RepairImpossible(bool isRepair)
        {
            partRepairButton.interactable = isRepair;
            partRepairButton.image.sprite = partRepairButtonSprites[isRepair ? 0 : 1];
        }

        // 수리 완료
        public void RepairClear()
        {

        }
    }
}
