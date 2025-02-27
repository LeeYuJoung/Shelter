using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        public Image dayImage;
        public Image timeImage;

        public Sprite[] daySprites;
        public Sprite[] timeSptites;

        public Text[] goldTexts;
        public Text[] robotBuyPriceTexts;
        public Text[] robotUpgradePriceTexts;
        public Text[] collectorRobotPieceTexts;
        public Text[] sweeperRobotPieceTexts;

        public GameObject[] robotRepairs;
        public Sprite[] robotRepairSprites;

        public GameObject[] robotUpgrades;
        public Sprite[] robotUpgradeSptites;

        public Sprite[] sweeperRobotSprites;
        public Sprite[] collectorRobotSprites;

        public GameObject raderRoom;
        public Sprite raderRoomSprites;

        public Sprite[] repairButtonSprites;

        public Button[] resolutionButtons;
        public Sprite[] selectResolutions;
        public Sprite[] deselectResolutions;

        public Button[] buttons;

        public int descriptionIndex = 0;
        public GameObject description;
        public Sprite[] descriptionSprites;

        public ScrollAnimation storeScrollAnimation;
        public ScrollAnimation statusScrollAnimation;
        public UIAnimation uiAnimation;

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

        private void Start()
        {
            Init();

            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(delegate { AudioManager.Instance.PlaySFX(0); });
            }

            for(int i = 0; i < resolutionButtons.Length; i++)
            {
                resolutionButtons[i].onClick.AddListener(delegate { AudioManager.Instance.PlaySFX(0); });
            }
        }

        public void Init()
        {
            for(int i = 0; i < goldTexts.Length; i++)
            {
                goldTexts[i].text = string.Format("x {0:D3}", GameManager.Instance.GetGold);
            }

            for(int i = 0; i < collectorRobotPieceTexts.Length; i++)
            {
                collectorRobotPieceTexts[i].text = string.Format("x {0}", GameManager.Instance.collectorRobots.Count);
                sweeperRobotPieceTexts[i].text = string.Format("x {0}", GameManager.Instance.sweeperRobots.Count);
            }
        }

        // 디데이 변경
        public void UpdateDayImage(int day)
        {
            dayImage.sprite = daySprites[day];
        }

        // 하루 시간 변경
        public void UpdateTimeImage(int time)
        {
            timeImage.sprite = timeSptites[time];
        }

        // 골드 텍스트 변경
        public void UpdateGoldText(int gold)
        {
            for(int i = 0; i < goldTexts.Length; i++)
            {
                goldTexts[i].text = string.Format("x {0:D3}", gold);
            }
        }

        // 로봇 구매 가격 텍스트 변경
        public void UpdateRobotBuyPriceText(int index, int price)
        {
            robotBuyPriceTexts[index].text = string.Format("x {0}", price);
        }

        // 로봇 업그레이드 가격 텍스트 변경
        public void UpdateRoboUpgradetPriceText(int index, int price)
        {
            robotUpgradePriceTexts[index].text = string.Format("x {0}", price);
        }

        // 로봇 수량 텍스트 변경
        public void UpdateRobotPieceText(int collectorRobotPiece, int sweeperRobotPiece)
        {
            for(int i = 0; i < collectorRobotPieceTexts.Length; i++)
            {
                collectorRobotPieceTexts[i].text = string.Format("x {0}", collectorRobotPiece);
                sweeperRobotPieceTexts[i].text = string.Format("x {0}", sweeperRobotPiece);
            }
        }

        // 업그레이드 상태 이미지 변경
        public void UpgradeState(int typeIndex)
        {
            if(typeIndex == 0)
            {
                robotUpgrades[typeIndex].transform.GetChild(1).GetComponent<Image>().sprite = sweeperRobotSprites[GameManager.Instance.sweeperRobotLevel];
            }
            else
            {
                robotUpgrades[typeIndex].transform.GetChild(1).GetComponent<Image>().sprite = collectorRobotSprites[GameManager.Instance.collectorRobotLevel];
            }
        }

        // 구매 완료
        public void SoldOut(int typeIndex, GameObject btn)
        {
            robotRepairs[typeIndex].GetComponent<Image>().sprite = robotRepairSprites[typeIndex];
            robotRepairs[typeIndex].transform.GetChild(0).gameObject.SetActive(true);

            btn.GetComponent<Button>().interactable = false;
        }

        // 업그레이드 완료
        public void UpgradeClear(int typeIndex, GameObject btn)
        {
            robotUpgrades[typeIndex].GetComponent<Image>().sprite = robotUpgradeSptites[typeIndex];
            robotUpgrades[typeIndex].transform.GetChild (0).gameObject.SetActive(true);

            btn.GetComponent <Button>().interactable = false;
        }

        // 레이더실 해금 완료
        public void RaderRoomUnLock(GameObject btn)
        {
            raderRoom.GetComponent<Image>().sprite = raderRoomSprites;
            raderRoom.transform.GetChild(0).gameObject.SetActive(true);

            btn.GetComponent<Button>().interactable = false;
        }

        // 수리 가능 여부 확인
        public void RepairPossible(GameObject btn, bool isPossible)
        {
            if(isPossible)
            {
                btn.GetComponent<Image>().sprite = repairButtonSprites[0];
                btn.GetComponent<Button>().interactable = true;
            }
            else
            {
                btn.GetComponent<Image>().sprite = repairButtonSprites[1];
                btn.GetComponent<Button>().interactable = false;
            }
        }

        // 해상도 관리
        public void SetResolution(int resolutionIndex)
        {
            for(int i = 0; i < resolutionButtons.Length; i++)
            {
                resolutionButtons[i].image.sprite = deselectResolutions[i];
            }
            resolutionButtons[resolutionIndex].image.sprite = selectResolutions[resolutionIndex];

            AudioManager.Instance.resolutionIndex = resolutionIndex;
            Resolution resolution = GameManager.Instance.resolutions[resolutionIndex];
            //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void DescriptionClear()
        {
            descriptionIndex = 0;
            description.transform.GetChild(0).GetComponent<Image>().sprite = descriptionSprites[descriptionIndex];
        }

        public void DescriptionImageChange(int arrow)
        {
            if(arrow == 0)
            {
                if(descriptionIndex >= 2)
                    descriptionIndex = 0;
                else
                    descriptionIndex++;

                description.transform.GetChild(0).GetComponent<Image>().sprite = descriptionSprites[descriptionIndex];
            }
            else
            {
                if (descriptionIndex <= 0)
                    descriptionIndex = 2;
                else
                    descriptionIndex--;

                description.transform.GetChild(0).GetComponent<Image>().sprite = descriptionSprites[descriptionIndex];
            }
        }

        // UI창 전부 닫기
        public void AllCLose()
        {
            description.SetActive(false);
            storeScrollAnimation.Close();
            statusScrollAnimation.Close();
            uiAnimation.Close();
        }
    }
}
