using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Donhyun.UI.Animation;
using System.Linq;
using MiniGame;
using Manager;

public class MiniGame_1 : MiniGameController
{
    [Header("----- MiniGameObjects -----")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private GameObject leftWireGroup;
    [SerializeField] private GameObject rightWireGroup;
    [SerializeField] private List<WireType> wireTypes;
    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private GameObject wireTargetPrefab;

    [Header("----- MiniGameTimer -----")]
    //[SerializeField] private Image Timer;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float maxTime;

    [Header("----- MiniGameMaxWIreValue -----")]
    [SerializeField] private int maxWireValue;

    [Header("----- UIAnimationInformation -----")]
    [SerializeField] UIInformation miniGameUIInfo;

    private List<GameObject> leftWireObjects;
    private List<GameObject> rightWireObjects;
    private int wireLength;
    private float currentTIme;
    private bool beginGame;
    private int correctAnswerValue;
    private int currentAnswerValue;
    private List<WireType> types;

    public Image resultImage;
    public Sprite[] resultSprites;

    public Sprite[] wireHeadSprites;
    public Sprite[] wireBodySprites;
    public Sprite[] wireTailSprites;

    public UIAnimation uiAnimation;
    public GameObject errorGameObject;
    private bool isClear = false;

    //프로퍼티
    public float MaxTime
    {
        get
        {
            return maxTime;
        }
        set
        {
            maxTime = value;
        }
    }

    private void Awake()
    {
        correctAnswerValue = 0;
        currentAnswerValue = 0;
        beginGame = false;
        wireLength = wireTypes.Count;
        //maxWireValue = Mathf.Min(maxWireValue, wireLength);
        maxWireValue = 4;
        leftWireObjects = new List<GameObject>();
        rightWireObjects = new List<GameObject>();

        maxTime = playTime;
    }

    private void Update()
    {
        if(beginGame)
        {
            TimeUpdate();
        }
    }

    public override void GetReward()
    {
        base.GetReward();
        if (StatusManager.Instance.status.statusData.EngineRestorationRate + reward <= 100)
        {
            StatusManager.Instance.partPrices[2] += 5;
            StatusManager.Instance.status.SetEngineRestorationRate(true);
        }
        else
            StatusManager.Instance.status.statusData.EngineRestorationRate = 100;

        if (StatusManager.Instance.status.statusData.EngineRestorationRate >= 100)
            StatusManager.Instance.RepairClear(2, StatusManager.Instance.engineImage, true);
    }

    public override void GetPenalty()
    {
        base.GetPenalty();
        if (StatusManager.Instance.status.statusData.EngineRestorationRate - penalty >= 0)
        {
            StatusManager.Instance.partPrices[2] -= 5;
            StatusManager.Instance.status.SetEngineRestorationRate(false);
        }
        else
            StatusManager.Instance.status.statusData.EngineRestorationRate = 0;

        if (StatusManager.Instance.status.statusData.EngineRestorationRate < 100)
            StatusManager.Instance.RepairClear(2, StatusManager.Instance.engineImage, false);
    }

    public override void OnBeep()
    {
        base.OnBeep();

        Debug.Log(":: MiniGame1 Beep ::");
        isError = false;
        errorGameObject.SetActive(false);
        GetPenalty();
    }

    public override void GameLevelUp()
    {
        base.GameLevelUp();
    }

    // 게임 강제 종료
    public override void ForcingGameOver()
    {
        base.ForcingGameOver();

        if (beginGame)
        {
            Debug.Log("::: MiniGame1 강제 종료 :::");

            AudioManager.Instance.PlaySFX(2);
            errorGameObject.SetActive(false);
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = resultSprites[1];
            uiAnimation.Close();

            isError = false;
            isPlaying = false;
            beginGame = false;
            GetPenalty();
        }
        else
        {
            errorGameObject.SetActive(false);
            resultImage.gameObject.SetActive(true);
            uiAnimation.Close();

            isError = false;
            isPlaying = false;
        }

        ClearList(leftWireObjects);
        ClearList(rightWireObjects);
        correctAnswerValue = 0;
        currentAnswerValue = 0;
    }

    //게임 실행
    public override void GameStart()
    {
        base.GameStart();
        
        miniGamePanel.SetActive(true);
        resultImage.gameObject.SetActive(false);

        beginGame = true;
        currentTIme = playTime;
        AddLeftWire(leftWireObjects, leftWireGroup);
        AddRightWire(leftWireObjects, rightWireObjects, rightWireGroup);
        correctAnswerValue += leftWireObjects.Count;

        Debug.Log("정답 개수 : " + correctAnswerValue);
    }

    //게임 종료시 실행
    public override void ClearGame()
    {
        base.ClearGame();

        //게임 성공
        if (isClear)
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = resultSprites[0];
            GetReward();
        }
        //게임 실패
        else
        {
            resultImage.gameObject.SetActive(true);
            resultImage.sprite = resultSprites[1];
            GetPenalty();
        }
    }

    //타이머 시간 감소
    private void TimeUpdate()
    {
        if(currentAnswerValue == correctAnswerValue)
        {
            beginGame = false;
            isClear = true;
            //ClearGame(true);
            ClearGame();
            return;
        }

        if (currentTIme > 0)
        {
            currentTIme -= Time.deltaTime;
        }
        else
        {
            currentTIme = playTime;
            beginGame = false;
            isClear = false;
            //ClearGame(false);
            ClearGame();
            return;
        }
        //Timer.fillAmount = currentTIme / maxTime;
        timerSlider.value = currentTIme / playTime;
    }

    //셔플(제네릭)
    private List<T> Shuffle<T>(List<T> values)
    {
        System.Random rand = new System.Random();
        var shuffled = values.OrderBy(_ => rand.Next()).ToList();

        return shuffled;
    }

    //정답 전선 추가
    private void AddLeftWire(List<GameObject> wireObjects, GameObject wireGroup)
    {
        int maxWireLength = Random.Range(wireLength - 2, wireLength);

        types = Shuffle(wireTypes);

        for (int i = 0; i < maxWireLength; i++)
        {
            GameObject wire = Instantiate(wirePrefab, wireGroup.transform);
            ChangeWireColor(types[i], wire, true);
            wire.GetComponent<LineStretch>().SetMiniGame(this);
            wireObjects.Add(wire);
        }
    }

    //오답 전선 추가
    private void AddRightWire(List<GameObject> AnswerWireObjectsList, List<GameObject> wireObjects, GameObject wireGroup)
    {
        int maxWireLength = Random.Range(0, 2);

        List<GameObject> list = new List<GameObject>();

        //정답 추가
        foreach (WireType type in types)
        {
            GameObject wire = Instantiate(wireTargetPrefab, wireGroup.transform);
            ChangeWireColor(type, wire, false);
            list.Add(wire);
        }

        //오답 추가
        for (int i = 0; i < maxWireLength; i++)
        {
            GameObject wire = Instantiate(wireTargetPrefab, wireGroup.transform);

            //전선 프리팹 중 랜덤
            //int random = UnityEngine.Random.Range(0, wireLength);
            int random = Random.Range(0, wireLength);
            ChangeWireColor((WireType)random, wire, false);
            list.Add(wire);
        }

        list = Shuffle(list);

        foreach (GameObject go in list)
        {
            wireObjects.Add(Instantiate(go, wireGroup.transform));
            Destroy(go); //기존 전선을 삭제
        }
    }

    //정답 개수 상승
    public void AnswerCorrectly()
    {
        currentAnswerValue++;
        //Debug.Log("정답 개수 증가 : " + currentAnswerValue);
    }

    //선 색 변경 및 타입 지정
    private void ChangeWireColor(WireType type, GameObject wire, bool ishead)
    {
        Image[] wireImages = wire.GetComponentsInChildren<Image>();

        Color color;
        wire.GetComponent<Wire>().WireType = type;
        int index = 0;

        switch (type)
        {
            case WireType.Red:
                color = Color.red;
                index = 0;
                break;
            case WireType.Blue:
                color = Color.blue;
                index = 1;
                break;
            case WireType.White:
                color = Color.white;
                index = 2;
                break;
            case WireType.Green:
                color = Color.green;
                index = 3;
                break;
            case WireType.Yellow:
                color = Color.yellow;
                index = 4;
                break;
            default:
                color = Color.red;
                index = 0;
                break;
        }

        //foreach(Image image in wireImages)
        //{
        //    image.color = color;
        //}

        for(int i = 0; i  < wireImages.Length; i++)
        {
            if (ishead)
            {
                if (i == 0)
                    wireImages[i].sprite = wireBodySprites[index];
                else
                    wireImages[i].sprite = wireHeadSprites[index];
            }
            else
            {
                if (i == 0)
                    wireImages[i].sprite = wireTailSprites[index];
                else
                    wireImages[i].sprite = wireHeadSprites[index];
            }
        }
    }

    //모든 전선 삭제
    private void ClearList(List<GameObject> wireObjects)
    {
        foreach (GameObject go in wireObjects)
        {
            Destroy(go);
        }

        wireObjects.Clear();
    }
}
