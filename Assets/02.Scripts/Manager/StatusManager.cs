using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class StatusManager : MonoBehaviour
    {
        private static StatusManager instance;
        public static StatusManager Instance { get { return instance; } }

        public Status status;

        public Text partNameText;          // 부품 이름
        public Image partImage;            // 부품 이미지
        public Text partDescriptionText;   // 부품 설명
        public Text partRepairPriceText;   // 부품 수리 비용
        public Slider partStatusGauge;     // 부품 스테이터스 게이지
        public Button partRepairButton;    // 부품 수리 버튼
        public Sprite[] partRepairButtonSprites;

        private int currentPartIndex;

        private string[] partNames;
        public Sprite[] partSprites;
        private string[] descriptions;
        private int[] partPrices;

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

            partNames = new string[] { "오염도", "선체", "모터", "엔진", "레이더", "레이더 출력량" };
            descriptions = new string[] {
                "오염도는 저어닐 밎ㄷㅂ잳 가나나 ㅁㄷㅇㅂㅈㄷ",
                "선체 복구도를 높이면 어쩌구 저저꺼주거 ㅇㅁㅇㅁㄴㅇ",
                "모터 복구도 ㅘ라란ㅇ리 넌 ㅇㅁㄴ오ㅑㅈㄷ",
                "엔진 복구도 블라라라 ㅇ너미쟈 ㅜㅁㅈㄷㅇ  ㅇㅁㅇ",
                "레이더 복구도 쏘라라라 ㅏ지ㅏ민ㅇ 어쩌구 저저ㅉ거 ㅁㅇ ㅁㄴㅇ",
                "레이더 출력랴으으ㅜㅁㄴ얌젿뱌더밎뎔기ㅓㅇㄹ ㅁㄴㅇㅁㄴㅇ  ㅁㄴㅇ "};
            partPrices = new int[] { 0, 1000, 1000, 1000, 1000, 0 };
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
            float gauge = 0;

            switch (partIndex)
            {
                case 0:
                    gauge = status.statusData.Corrosion;
                    RepairImpossible(false);
                    break;
                case 1:
                    gauge = status.statusData.HullRestorationRate;
                    RepairImpossible(true);
                    break;
                case 2:
                    gauge = status.statusData.MotorRestorationRate;
                    RepairImpossible(true);
                    break;
                case 3:
                    gauge = status.statusData.EngineRestorationRate;
                    RepairImpossible(true);
                    break;
                case 4:
                    gauge = status.statusData.RadarRestorationRate;
                    RepairImpossible(true);
                    break;
                case 5:
                    gauge = status.statusData.RadarOutputAmount;
                    RepairImpossible(false);
                    break;
            }

            partStatusGauge.value = gauge;
        }

        // Status UI에서 수리 버튼 클릭 시 실행
        public void Reapir()
        {

        }

        // 수리 가능 or 불가능 확인
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
