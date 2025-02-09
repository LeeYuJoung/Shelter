using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MiniGame;
using Manager;

public class MiniGame_2 : MiniGameController
{
    public GameObject miniGame2GameObject;
    public GameObject errorGameObject;

    public Slider timeSlider; // 시간 슬라이더
    public Slider powerSlider; // 전력 게이지 슬라이더
    public TextMeshProUGUI resultText; // 결과 메시지 표시
    public AudioSource wrongInputSound; // 틀린 입력 시 재생되는 소리
    public Transform arrowParent; // 화살표 부모 오브젝트
    public GameObject arrowPrefab; // 화살표 프리팹

    private float maxTime;
    private float miniGame2_currentTime; // 남은 시간
    private float maxPower = 100f; // 최대 전력 게이지
    private float currentPower = 0; // 현재 전력 게이지
    private bool isGameActive = false; // 게임 진행 여부 (초기값: false)
    private string[] currentArrowKeys = new string[4]; // 현재 표시된 방향키 배열
    private int currentInputIndex = 0; // 플레이어가 입력 중인 방향키 인덱스

    public override void GetReward()
    {
        base.GetReward();
        StatusManager.Instance.status.SetMotorRestorationRate(true);
    }

    public override void GetPenalty()
    {
        base.GetPenalty();
        StatusManager.Instance.status.SetMotorRestorationRate(false);
    }

    public override void OnBeep()
    {
        base.OnBeep();

        Debug.Log(":: MiniGame2 Beep ::");
        isError = false;
        errorGameObject.SetActive(false);
        StatusManager.Instance.status.SetMotorRestorationRate(false);
    }

    public override void GameLevelUp()
    {
        base.GameLevelUp();
    }

    public override void GameStart()
    {
        base.GameStart();
        miniGame2GameObject.SetActive(true);

        isGameActive = true; // ✅ 게임 활성화
        maxTime = playTime;
        miniGame2_currentTime = playTime; // ✅ 제한 시간 초기화
        currentPower = 0; // ✅ 전력 게이지 초기화
        currentInputIndex = 0; // ✅ 입력 인덱스 초기화

        // ✅ 결과 메시지 숨기기
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
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

        GenerateRandomArrowKeys(); // ✅ 랜덤 방향키 생성

        Debug.Log("🎮 미니게임 시작!");
    }

    void Update()
    {
        if (!isError)
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
            Transform currentArrow = arrowParent.GetChild(currentInputIndex);
            currentArrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.green; // 초록색으로 변경
            currentInputIndex++; // 다음 방향키로 이동

            // 모든 방향키를 정확히 입력한 경우
            if (currentInputIndex >= currentArrowKeys.Length)
            {
                currentInputIndex = 0;
                GenerateRandomArrowKeys(); // 새로운 랜덤 방향키 생성
                currentPower = Mathf.Min(currentPower + plusPoint, maxPower); // 전력 게이지 증가
            }
        }
        else if (Input.anyKeyDown) // 잘못된 키 입력 시
        {
            PlayWrongInputSound(); // 틀린 입력 소리 재생
            StartCoroutine(WrongInputFeedback()); // 잘못된 입력 시 피드백 연출
            currentPower = Mathf.Max(currentPower - minusPoint, 0); // 전력 게이지 감소
        }
    }

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
            currentArrowKeys[i] = arrowKeys[Random.Range(0, arrowKeys.Length)];

            // 화살표 UI 생성
            GameObject arrow = Instantiate(arrowPrefab, arrowParent);
            TextMeshProUGUI arrowText = arrow.GetComponentInChildren<TextMeshProUGUI>();
            arrowText.text = currentArrowKeys[i];
            arrowText.color = Color.white; // 기본 색상 흰색
        }
    }

    IEnumerator WrongInputFeedback()
    {
        // 모든 화살표를 빨간색으로 깜빡이기
        foreach (Transform child in arrowParent)
        {
            TextMeshProUGUI arrowText = child.GetComponentInChildren<TextMeshProUGUI>();
            arrowText.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        // 다시 랜덤 방향키 생성
        currentInputIndex = 0; // 🎯 인덱스 초기화 (이게 빠져있어서 오류 발생 가능)
        GenerateRandomArrowKeys(); // 🎯 새로운 방향키 생성
    }

    void FailGame()
    {
        isGameActive = false; // 게임 중지
        resultText.gameObject.SetActive(true);
        resultText.text = "Game Over!"; // 실패 메시지 표시
        Debug.Log("❌ 게임 실패!");
    }

    void SuccessGame()
    {
        isGameActive = false; // 게임 중지
        resultText.gameObject.SetActive(true);
        resultText.text = "Game Clear!"; // 성공 메시지 표시
        Debug.Log("🎉 게임 성공!");
    }

    void PlayWrongInputSound()
    {
        if (wrongInputSound != null)
        {
            wrongInputSound.Play();
        }
    }

    public void ForcingGameOver()
    {
        if(isGameActive)
        {
            Debug.Log(":: MiniGame2 강제 종료 ::");

            isGameActive = false; // 게임 중지
            resultText.gameObject.SetActive(true);
            resultText.text = "Game Over!"; // 실패 메시지 표시
            Debug.Log("❌ 게임 실패!");

            isPlaying = false;
            errorGameObject.SetActive(false);
            StatusManager.Instance.status.SetMotorRestorationRate(false);
        }
    }
}
