using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MiniGame;
using Manager;

public class MiniGame_3 : MiniGameController
{
    public GameObject miniGame3GameObject;
    public GameObject errorGameObject;

    public Image spaceGauge;   // ✅ SpaceGauge 내부에서만 이동해야 함
    public Image greenZone;
    public Image yellowZone;
    public Image indicator;
    public Image donutGauge;   // ✅ 도넛 게이지 (시각적 점수 표시)
    public Text gameOverText;  // ✅ 게임 종료 메시지
    public Slider timeSlider;  // ✅ 10초 타이머 (슬라이드바)

    private float indicatorSpeed = 250; // ✅ 이동 속도
    private int direction = 1;          // ✅ 이동 방향 (1: 위로, -1: 아래로)
    private bool isGameOver = false;    // ✅ 게임 종료 여부

    private int totalScore = 0;  // ✅ 총 점수 (최대 100점) : 게이지 수치용
    private float maxTime;
    private float miniGame3_currentTime;  // ✅ 현재 남은 시간

    private int greenBonus = 20;  // ✅ 초록색 영역 점수
    private int yellowBonus = 5;  // ✅ 노란색 영역 점수
    private int redPenalty = -5;  // ✅ 빨간색 영역 점수

    void Start()
    {
        isGameOver = true;
        maxTime = playTime;
        miniGame3_currentTime = playTime; // ✅ 제한 시간 초기화
    }

    public override void GetReward()
    {
        base.GetReward();
        if (StatusManager.Instance.status.statusData.RadarRestorationRate + reward <= 100)
            StatusManager.Instance.status.SetRadarRestorationRate(true);
        else
            StatusManager.Instance.status.statusData.RadarRestorationRate = 100;
    }

    public override void GetPenalty()
    {
        base.GetPenalty();
        if (StatusManager.Instance.status.statusData.RadarRestorationRate - penalty >= 0)
            StatusManager.Instance.status.SetRadarRestorationRate(false);
        else
            StatusManager.Instance.status.statusData.RadarRestorationRate = 0;
    }

    public override void OnBeep()
    {
        base.OnBeep();

        Debug.Log(":: MiniGame3 Beep ::");
        isError = false;
        errorGameObject.SetActive(false);
        GetPenalty();
    }

    public override void GameLevelUp()
    {
        base.GameLevelUp();
        indicatorSpeed += 50.0f;
    }

    public override void GameStart()
    {
        base.GameStart();
        miniGame3GameObject.SetActive(true);

        //indicatorSpeed = 200f; // ✅ 기본 이동 속도 설정
        isGameOver = false; // ✅ 게임 진행 가능하도록
        maxTime = playTime;
        miniGame3_currentTime = playTime; // ✅ 제한 시간 초기화
        totalScore = 0; // ✅ 점수 초기화
        direction = 1; // ✅ 위쪽 이동 방향 설정
        indicator.rectTransform.anchoredPosition = new Vector2(0, 0); // ✅ Indicator 위치 초기화

        if (timeSlider != null)
        {
            timeSlider.maxValue = 1; // ✅ 타이머 최대값 설정
            timeSlider.value = 1; // ✅ 현재 값 업데이트
        }

        RandomizeZones(); // ✅ GreenZone, YellowZone 랜덤 배치
        UpdateGauge(); // ✅ 도넛 게이지 초기화

        // ✅ 게임 종료 메시지 숨기기
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        Debug.Log("🎮 게임 시작!");
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

        if (!isGameOver)
        {
            MoveIndicator(); // ✅ `Indicator` 이동
            UpdateTimer();   // ✅ 타이머 업데이트

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckHit(); // ✅ SpaceBar 입력 시 판정 실행
            }
        }
    }

    void MoveIndicator()
    {
        float moveAmount = indicatorSpeed * direction * Time.deltaTime;

        // ✅ 현재 위치에서 Y축만 이동
        Vector2 newPosition = indicator.rectTransform.anchoredPosition;
        newPosition.y += moveAmount;

        // ✅ SpaceGauge 내부에서만 이동하도록 제한
        float maxY = (spaceGauge.rectTransform.rect.height / 2) - (indicator.rectTransform.rect.height / 2);

        // ✅ 범위를 벗어나면 방향 반전
        if (newPosition.y >= maxY)
        {
            direction = -1; // 위쪽 끝에서 아래로 이동
            newPosition.y = maxY;
        }
        else if (newPosition.y <= -maxY)
        {
            direction = 1; // 아래쪽 끝에서 위로 이동
            newPosition.y = -maxY;
        }

        // ✅ 최종 위치 적용
        indicator.rectTransform.anchoredPosition = newPosition;
    }

    void RandomizeZones()
    {
        float gaugeHeight = spaceGauge.rectTransform.rect.height;
        float yellowHeight = yellowZone.rectTransform.rect.height;
        float greenHeight = greenZone.rectTransform.rect.height;

        // ✅ `YellowZone`이 `SpaceGauge` 내부에서 랜덤 위치로 배치됨
        float yellowMinY = -gaugeHeight / 2 + yellowHeight / 2;  // 아래쪽 제한
        float yellowMaxY = gaugeHeight / 2 - yellowHeight / 2;   // 위쪽 제한
        float yellowY = Random.Range(yellowMinY, yellowMaxY);
        yellowZone.rectTransform.anchoredPosition = new Vector2(0, yellowY);

        // ✅ `GreenZone`은 `YellowZone` 내부에서만 이동하도록 설정
        float greenMinY = yellowY - (yellowHeight / 2) + greenHeight / 2;
        float greenMaxY = yellowY + (yellowHeight / 2) - greenHeight / 2;
        float greenY = Random.Range(greenMinY, greenMaxY);
        greenZone.rectTransform.anchoredPosition = new Vector2(0, greenY);

        Debug.Log("✅ 새 위치 설정 - YellowZone: " + yellowY + " / GreenZone: " + greenY);
    }

    void CheckHit()
    {
        if (isGameOver) return;

        float indicatorY = indicator.rectTransform.anchoredPosition.y;

        float greenMin = greenZone.rectTransform.anchoredPosition.y - (greenZone.rectTransform.sizeDelta.y / 2);
        float greenMax = greenZone.rectTransform.anchoredPosition.y + (greenZone.rectTransform.sizeDelta.y / 2);
        float yellowMin = yellowZone.rectTransform.anchoredPosition.y - (yellowZone.rectTransform.sizeDelta.y / 2);
        float yellowMax = yellowZone.rectTransform.anchoredPosition.y + (yellowZone.rectTransform.sizeDelta.y / 2);

        if (indicatorY >= greenMin && indicatorY <= greenMax)
        {
            totalScore += greenBonus;
            Debug.Log("✅ GreenZone: + " + greenBonus + "점");
        }
        else if (indicatorY >= yellowMin && indicatorY <= yellowMax)
        {
            totalScore += yellowBonus;
            Debug.Log("⚠️ YellowZone: + " + yellowBonus + "점");
        }
        else
        {
            totalScore += redPenalty;
            Debug.Log("❌ RedZone: " + redPenalty + "점");
        }

        totalScore = Mathf.Clamp(totalScore, 0, 100);
        UpdateGauge();

        if (totalScore < 100)
        {
            RandomizeZones();
        }
        else
        {
            ClearGame();
        }
    }

    void UpdateTimer()
    {
        miniGame3_currentTime -= Time.deltaTime;
        timeSlider.value = (miniGame3_currentTime / maxTime);

        if (miniGame3_currentTime <= 0)
        {
            ClearGame();
        }
    }

    void UpdateGauge()
    {
        if (donutGauge != null)
        {
            donutGauge.fillAmount = totalScore / 100f;
        }
    }

    public override void ClearGame()
    {
        base.ClearGame();
        isGameOver = true;
        Debug.Log("🎮 게임 종료! 점수 100 도달 또는 시간 종료!");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            if (totalScore >= 100)
            {
                GetReward();
                gameOverText.text = "🎉 Clear!";
                gameOverText.color = Color.green;
            }
            else
            {
                GetPenalty();
                gameOverText.text = "❌ Game Over!";
                gameOverText.color = Color.red;
            }
        }

        errorGameObject.SetActive(false);
    }

    // 게임 강제 종료 시 실행
    public void ForcingGameOver()
    {
        if(!isGameOver)
        {
            Debug.Log(":: MiniGame3 강제 종료 ::");

            isGameOver = true;
            gameOverText.gameObject.SetActive(true);

            gameOverText.text = "❌ Game Over!";
            gameOverText.color = Color.red;

            isPlaying = false;
            errorGameObject.SetActive(false);
            GetPenalty();
        }
    }
}
