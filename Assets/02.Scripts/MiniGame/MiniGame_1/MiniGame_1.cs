using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Donhyun.UI.Animation;
public class MiniGame_1 : MonoBehaviour
{
    [Header("----- MiniGameObjects -----")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private GameObject leftWireGroup;
    [SerializeField] private GameObject rightWireGroup;
    [SerializeField] private GameObject[] wire;

    [Header("----- MiniGameTimer -----")]
    [SerializeField] private Image Timer;
    [SerializeField] float maxTime;

    [Header("----- UIAnimationInformation -----")]
    [SerializeField] UIInformation miniGameUIInfo;

    private List<GameObject> leftWireObjects;
    private List<GameObject> rightWireObjects;
    private int wireLength;
    private float currentTIme;
    private bool beginGame;
    private int[] leftWireValue;
    private int[] rightWireValue;
    private int correctAnswerValue;
    private int currentAnswerValue;

    private void Awake()
    {
        correctAnswerValue = 0;
        currentAnswerValue = 0;
        beginGame = false;
        wireLength = wire.Length;
        leftWireObjects = new List<GameObject>();
        rightWireObjects = new List<GameObject>();
        leftWireValue = new int[wireLength];
        rightWireValue = new int[wireLength];
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
        AddWire(leftWireObjects, leftWireGroup, leftWireValue);
        AddWire(rightWireObjects, rightWireGroup, rightWireValue);
        for (int i = 0; i < wireLength; i++)
        {
            correctAnswerValue += Mathf.Min(leftWireValue[i], rightWireValue[i]);
        }

        Debug.Log("정답 개수 : " + correctAnswerValue);
    }

    //게임 종료시 실행
    public void ClearGame(bool clear)
    {
        UIAnimationManager.CloseUI(() => { 
            miniGamePanel.SetActive(false);
            ClearList(leftWireObjects, leftWireValue);
            ClearList(rightWireObjects, rightWireValue);
            correctAnswerValue = 0;
            currentAnswerValue = 0;

            if (clear)
            {

            }
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

    //그룹에 와이어 랜덤 추가
    private void AddWire(List<GameObject> wireObjects, GameObject wireGroup, int[] wireValue)
    {
        int maxWireLength = UnityEngine.Random.Range(wireLength-2, wireLength);

        for (int i = 0; i < maxWireLength; i++)
        {
            int random = UnityEngine.Random.Range(0, wireLength - 1);
            GameObject go = Instantiate(wire[random], wireGroup.transform);
            wireValue[(int)go.GetComponent<Wire>().WireType]++;
            wireObjects.Add(go);
        }
    }

    //모든 전선 삭제
    private void ClearList(List<GameObject> wireObjects, int[] wireValue)
    {
        foreach (GameObject go in wireObjects)
        {
            Destroy(go);
        }

        for(int i = 0; i < wireLength; i++)
        {
            wireValue[i] = 0;
        }

        wireObjects.Clear();
    }
}
