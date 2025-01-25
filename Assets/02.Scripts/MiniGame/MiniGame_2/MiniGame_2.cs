using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGame02 : MonoBehaviour
{
    public Slider timeSlider; // 시간 슬라이더
    public Slider powerSlider; // 전력 게이지 슬라이더
    public TextMeshProUGUI arrowDisplay; // 방향키 표시
    public TextMeshProUGUI resultText; // 결과 메시지 표시
    public AudioSource wrongInputSound; // 틀린 입력 시 재생되는 소리
    public Transform arrowParent;
    public GameObject arrowPrefab;

    private float maxTime = 10f; // 제한 시간
    private float currentTime; // 남은 시간
    private float maxPower = 100f; // 최대 전력 게이지
    private float currentPower; // 현재 전력 게이지
    private bool isGameActive = true; // 게임 진행 여부
    private string[] currentArrowKeys = new string[4]; // 현재 표시된 방향키 배열
    private int currentInputIndex = 0; // 플레이어가 입력 중인 방향키 인덱스

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!isGameActive) return;

        // 시간 및 게이지 감소
        currentTime -= Time.deltaTime;
        currentPower -= Time.deltaTime * 1f; // 전력 게이지도 천천히 감소
        timeSlider.value = currentTime / maxTime;
        powerSlider.value = currentPower / maxPower;

        // 시간 초과 또는 게이지가 0이 되면 실패 처리
        if (currentTime <= 0 || currentPower <= 0)
        {
            FailGame();
        }

        // 방향키 입력 체크
        CheckInput();
    }

    void StartGame()
    {
        // 초기화
        isGameActive = true;
        currentTime = maxTime;
        currentPower = maxPower;
        resultText.gameObject.SetActive(false);
        GenerateRandomArrowKeys(); // 랜덤 방향키 생성
    }

    void CheckInput()
    {
        if (!isGameActive) return;

        // 방향키 입력 처리
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentArrowKeys[currentInputIndex] == "↑" ||
            Input.GetKeyDown(KeyCode.DownArrow) && currentArrowKeys[currentInputIndex] == "↓" ||
            Input.GetKeyDown(KeyCode.LeftArrow) && currentArrowKeys[currentInputIndex] == "←" ||
            Input.GetKeyDown(KeyCode.RightArrow) && currentArrowKeys[currentInputIndex] == "→")
        {
            // 현재 방향키가 올바른 경우
            Transform currentArrow = arrowParent.GetChild(currentInputIndex); // 현재 화살표 가져오기
            currentArrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.green; // 초록색으로 변경
            currentInputIndex++; // 다음 방향키로 이동


            // 모든 방향키를 정확히 입력한 경우
            if (currentInputIndex >= currentArrowKeys.Length)
            {
                currentInputIndex = 0;
                GenerateRandomArrowKeys(); // 새로운 랜덤 방향키 생성
                powerSlider.value += 20f; // 전력 게이지 회복
                if (currentPower > maxPower) currentPower = maxPower; // 최대 전력 게이지 제한
                //if (powerSlider.value > 1) powerSlider.value = 1; // 게이지 최대값 제한
            }
        }
        else if (Input.anyKeyDown) // 잘못된 키 입력 시
        {
            PlayWrongInputSound(); // 틀린 입력 소리 재생
            StartCoroutine(WrongInputFeedback()); // 잘못된 입력 시 피드백 연출
            //currentInputIndex = 0; // 입력 초기화
            //GenerateRandomArrowKeys(); // 새로운 랜덤 방향키 생성
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
            arrowText.color = Color.white; // 기본 색상은 흰색
        }

        currentInputIndex = 0; // 입력 인덱스 초기화
        // 방향키를 화면에 표시
        //arrowDisplay.text = string.Join(" ", currentArrowKeys);
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
        GenerateRandomArrowKeys();
    }


    void FailGame()
    {
        isGameActive = false; // 게임 중지
        resultText.gameObject.SetActive(true);
        resultText.text = "Game over!"; // 실패 메시지 표시
    }

    void PlayWrongInputSound()
    {
        if (wrongInputSound != null)
        {
            wrongInputSound.Play();
        }
    }
}