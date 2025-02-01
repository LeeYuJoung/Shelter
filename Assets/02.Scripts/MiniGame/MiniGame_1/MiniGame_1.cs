using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Donhyun.UI.Animation;
using System.Linq;

public class MiniGame_1 : MonoBehaviour
{
    [Header("----- MiniGameObjects -----")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private GameObject leftWireGroup;
    [SerializeField] private GameObject rightWireGroup;
    [SerializeField] private List<GameObject> wires;

    [Header("----- MiniGameTimer -----")]
    [SerializeField] private Image Timer;
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
        wireLength = wires.Count;
        maxWireValue = Mathf.Min(maxWireValue, wireLength);
        leftWireObjects = new List<GameObject>();
        rightWireObjects = new List<GameObject>();
    }

    private void Update()
    {
        if(beginGame)
        {
            TimeUpdate();
        }
    }

    //게임 실행
    public void GameStart()
    {
        UIAnimationManager.OpenUI(() => { miniGamePanel.SetActive(true); }, miniGameUIInfo, AnimationType.PopUp);

        beginGame = true;
        currentTIme = maxTime;
        AddLeftWire(leftWireObjects, leftWireGroup);
        AddRightWire(leftWireObjects, rightWireObjects, rightWireGroup);
        correctAnswerValue += leftWireObjects.Count;

        Debug.Log("정답 개수 : " + correctAnswerValue);


    }

    //게임 종료시 실행
    public void ClearGame(bool clear)
    {
        UIAnimationManager.CloseUI(() => { 
            miniGamePanel.SetActive(false);
            ClearList(leftWireObjects);
            ClearList(rightWireObjects);
            correctAnswerValue = 0;
            currentAnswerValue = 0;

            //게임 성공
            if (clear)
            {

            }
            //게임 실패
            else
            {

            }

        }, miniGameUIInfo, AnimationType.PopUp);
    }

    //타이머 시간 감소
    private void TimeUpdate()
    {
        if(currentAnswerValue == correctAnswerValue)
        {
            ClearGame(true);
            return;
        }

        if (currentTIme > Mathf.Epsilon)
        {
            currentTIme -= Time.unscaledDeltaTime;
        }
        else
        {
            currentTIme = 0.0f;
            beginGame = false;
            ClearGame(false);
            return;
        }
        Timer.fillAmount = currentTIme / maxTime;
    }

    //셔플
    private List<GameObject> Shuffle(List<GameObject> values)
    {
        System.Random rand = new System.Random();
        var shuffled = values.OrderBy(_ => rand.Next()).ToList();

        return shuffled;
    }

    //정답 전선 추가
    private void AddLeftWire(List<GameObject> wireObjects, GameObject wireGroup)
    {
        int maxWireLength = UnityEngine.Random.Range(wireLength - 2, wireLength);

        List<GameObject> list = Shuffle(wires);

        for (int i = 0; i < maxWireLength; i++)
        {
            GameObject go = Instantiate(list[i], wireGroup.transform);
            wireObjects.Add(go);
        }
    }

    //오답 전선 추가
    private void AddRightWire(List<GameObject> AnswerWireObjectsList, List<GameObject> wireObjects, GameObject wireGroup)
    {
        int maxWireLength = UnityEngine.Random.Range(0, 3);

        List<GameObject> list = new List<GameObject>();
        //정답 추가
        foreach(GameObject go in AnswerWireObjectsList)
        {
            list.Add(go);
        }
        //오답 추가
        for (int i = 0; i < maxWireLength; i++)
        {
            //전선 프리팹 중 랜덤
            int random = UnityEngine.Random.Range(0, wires.Count);
            list.Add(wires[random]);
        }

        list = Shuffle(list);

        foreach (GameObject go in list)
        {
            wireObjects.Add(Instantiate(go, wireGroup.transform));
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
