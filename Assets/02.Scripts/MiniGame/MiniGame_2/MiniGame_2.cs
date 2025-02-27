using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MiniGame;
using Manager;

public class MiniGame_2 : MiniGameController
{
    public GameObject miniGame2GameObject;
    public GameObject errorGameObject;
    public UIAnimation uiAnimation;

    public Slider timeSlider;     // 시간 슬라이더
    public Slider powerSlider;    // 전력 게이지 슬라이더
    public Image resultImage;     // 결과 Image 표시
    public Sprite[] resultSprites;

    public Transform arrowParent;      // 화살표 부모 오브젝트
    public GameObject arrowPrefab;     // 화살표 프리팹
    public Sprite[] arrowSprites;      // 화살표 기본 이미지
    public Sprite[] arrowClearSprites; // 화살표 성공 이미지
    public Sprite[] arrowFailSprites;  // 화살표 실패 이미지

    private float maxTime;
    private float miniGame2_currentTime; // 남은 시간
    private float maxPower = 100f; // 최대 전력 게이지
    private float currentPower = 0; // 현재 전력 게이지
    private bool isGameActive = false; // 게임 진행 여부 (초기값: false)
    private string[] currentArrowKeys = new string[4]; // 현재 표시된 방향키 배열
    private int[] currentArrowIntKeys = new int[4];
    private int currentInputIndex = 0; // 플레이어가 입력 중인 방향키 인덱스

    public override void GetReward()
    {
        base.GetReward();

        if (StatusManager.Instance.status.statusData.MotorRestorationRate + reward <= 100)
        {
            StatusManager.Instance.partPrices[1] += 5;
            StatusManager.Instance.status.SetMotorRestorationRate(true);
        }
        else
            StatusManager.Instance.status.statusData.MotorRestorationRate = 100;

        if (StatusManager.Instance.status.statusData.MotorRestorationRate >= 100)
            StatusManager.Instance.RepairClear(1, StatusManager.Instance.moterImage, true);
    }

    public override void GetPenalty()
    {
        base.GetPenalty();

        if (StatusManager.Instance.status.statusData.MotorRestorationRate - penalty >= 0)
        {
            StatusManager.Instance.partPrices[1] -= 5;
            StatusManager.Instance.status.SetMotorRestorationRate(false);
        }
        else
            StatusManager.Instance.status.statusData.MotorRestorationRate = 0;

        if (StatusManager.Instance.status.statusData.MotorRestorationRate < 100)
            StatusManager.Instance.RepairClear(1, StatusManager.Instance.moterImage, false);
    }

    public override void OnBeep()
    {
        base.OnBeep();

        Debug.Log(":: MiniGame2 Beep ::");
        isError = false;
        audioSource.Stop();
        errorGameObject.SetActive(false);
        GetPenalty();
    }

    public override void GameLevelUp()
    {
        base.GameLevelUp();
    }

    public override void GameStart()
    {
        base.GameStart();
        audioSource.Stop();
        miniGame2GameObject.SetActive(true);
        GenerateRandomArrowKeys(); // ✅ 랜덤 방향키 생성

        isGameActive = true; // ✅ 게임 활성화
        maxTime = playTime;
        miniGame2_currentTime = playTime; // ✅ 제한 시간 초기화
        currentPower = 0; // ✅ 전력 게이지 초기화
        currentInputIndex = 0; // ✅ 입력 인덱스 초기화

        // ✅ 결과 메시지 숨기기
        if (resultImage != null)
        {
            resultImage.gameObject.SetActive(false);
        }

        // ✅ UI 슬라이더 초기화
        if (timeSlider != null)
        {
            timeSlider.maxValue = 1;
            timeSlider.value = 1;
        }
        if (powerSlider != null)
        {
            powerSlider.maxValue = 1;
            powerSlider.value = 0;
        }
    }

    // 게임 강제 종료
    public override void ForcingGameOver()
    {
        base.ForcingGameOver();

        if (isGameActive)
        {
            Debug.Log(":: MiniGame2 강제 종료 ::");

            AudioManager.Instance.PlaySFX(2);
            isGameActive = false; // 게임 중지
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = resultSprites[1];

            isPlaying = false;
            errorGameObject.SetActive(false);
            uiAnimation.Close();
            GetPenalty();
        }
        else
        {
            audioSource.Stop();
            uiAnimation.Close();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isError || GameManager.Instance.isGameOver)
        {
            currentTime = 0;
            return;
        }

        currentTime += Time.deltaTime;

        if (currentTime >= beepTime)
        {
            currentTime = 0;

            if (!isPlaying)
            {
                OnBeep();
            }
        }

        if(isGameActive)
        {
            // 시간 감소
            miniGame2_currentTime -= Time.deltaTime;
            timeSlider.value = miniGame2_currentTime / maxTime;
            powerSlider.value = currentPower / maxPower;

            // 게임 상태 체크 (성공 또는 실패)
            ClearGame();

            // 방향키 입력 체크
            CheckInput();
        }
    }

    public override void ClearGame()
    {
        if (miniGame2_currentTime <= 0)
        {
            FailGame();
            GetPenalty();
            base.ClearGame();
            errorGameObject.SetActive(false);
        }
        else if (currentPower >= maxPower)
        {
            SuccessGame();
            GetReward();
            base.ClearGame();
            errorGameObject.SetActive(false);
        }
    }

    void CheckInput()
    {
        if (!isGameActive) 
            return;

        // 방향키 입력 처리
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentArrowKeys[currentInputIndex] == "↑" ||
            Input.GetKeyDown(KeyCode.DownArrow) && currentArrowKeys[currentInputIndex] == "↓" ||
            Input.GetKeyDown(KeyCode.LeftArrow) && currentArrowKeys[currentInputIndex] == "←" ||
            Input.GetKeyDown(KeyCode.RightArrow) && currentArrowKeys[currentInputIndex] == "→")
        {
            // 현재 방향키가 올바른 경우
            AudioManager.Instance.PlaySFX(7);
            Transform currentArrow = arrowParent.GetChild(currentInputIndex);
            currentArrow.GetComponent<PushAnimation>().OnSuccess();
            currentArrow.GetComponentInChildren<Image>().sprite = arrowClearSprites[currentArrowIntKeys[currentInputIndex]];

            //currentArrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.green; // 초록색으로 변경
            currentInputIndex++; // 다음 방향키로 이동

            // 모든 방향키를 정확히 입력한 경우
            if (currentInputIndex >= currentArrowKeys.Length)
            {
                currentInputIndex = 0;
                GenerateRandomArrowKeys(); // 새로운 랜덤 방향키 생성
                currentPower = Mathf.Min(currentPower + plusPoint, maxPower); // 전력 게이지 증가
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentArrowKeys[currentInputIndex] != "↑" ||
            Input.GetKeyDown(KeyCode.DownArrow) && currentArrowKeys[currentInputIndex] != "↓" ||
            Input.GetKeyDown(KeyCode.LeftArrow) && currentArrowKeys[currentInputIndex] != "←" ||
            Input.GetKeyDown(KeyCode.RightArrow) && currentArrowKeys[currentInputIndex] != "→") // 잘못된 키 입력 시
        {
            AudioManager.Instance.PlaySFX(8);
            StartCoroutine(WrongInputFeedback()); // 잘못된 입력 시 피드백 연출
            currentPower = Mathf.Max(currentPower - minusPoint, 0); // 전력 게이지 감소
        }
    }

    // 랜덤 화살표 생성
    void GenerateRandomArrowKeys()
    {
        // 기존 화살표 삭제
        foreach (Transform child in arrowParent)
        {
            Destroy(child.gameObject);
        }

        // 랜덤 방향키 4개 생성
        string[] arrowKeys = { "↑", "↓", "←", "→" };
        for (int i = 0; i < currentArrowKeys.Length; i++)
        {
            int rndIndex = Random.Range(0, arrowKeys.Length);
            currentArrowKeys[i] = arrowKeys[rndIndex];
            currentArrowIntKeys[i] = rndIndex;

            // 화살표 UI 생성
            GameObject arrow = Instantiate(arrowPrefab, arrowParent);
            arrow.GetComponent<Image>().sprite = arrowSprites[rndIndex];

            //TextMeshProUGUI arrowText = arrow.GetComponentInChildren<TextMeshProUGUI>();
            //arrowText.text = currentArrowKeys[i];
            //arrowText.color = Color.white; // 기본 색상 흰색
        }
    }

    // 잘못 눌렀을 경우
    IEnumerator WrongInputFeedback()
    {
        // 모든 화살표를 빨간색으로 깜빡이기
        foreach (Transform child in arrowParent)
        {
            child.GetComponent<Image>().sprite = arrowFailSprites[1];
            //TextMeshProUGUI arrowText = child.GetComponentInChildren<TextMeshProUGUI>();
            //arrowText.color = Color.red;
        }

        currentInputIndex = 0;     // 🎯 인덱스 초기화 (이게 빠져있어서 오류 발생 가능)
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        GenerateRandomArrowKeys(); // 🎯 새로운 방향키 생성
    }

    void FailGame()
    {
        isGameActive = false; // 게임 중지
        resultImage.gameObject.SetActive(true);
        resultImage.sprite = resultSprites[1];
    }

    void SuccessGame()
    {
        isGameActive = false; // 게임 중지
        resultImage.gameObject.SetActive(true);
        resultImage.sprite = resultSprites[0];
    }

    void PlayWrongInputSound()
    {
        //if (wrongInputSound != null)
        //{
        //    wrongInputSound.Play();
        //}
    }
}
